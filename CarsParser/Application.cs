namespace CarsParser
{
    using Data;
    using Newtonsoft.Json;
    using Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;

    public class Application : IApplication
    {
        private const string carUrl = "https://www.copart.co.uk/public/data/lotdetails/solr/{0}";
        private const string imagesUrl = "https://www.copart.co.uk/public/data/lotdetails/solr/lotImages/{0}";

        private const string lotsPath = @"C:\Users\izahariev\Desktop\lots\";
        private const string bidsPath = @"C:\Users\izahariev\Documents\twork\daniauto data\bids\";

        private readonly IUrlsService<Image> imagesService;
        private readonly IBaseService<Bid> bidsService;

        public Application(
            IValuesService<Make> makesService,
            IValuesService<Model> modelsService,
            IValuesService<Category> categoriesService,
            IValuesService<Location> locationsService,
            IValuesService<Currency> currenciesService,
            IValuesService<Transmission> transmissionsService,
            IValuesService<Fuel> fuelsService,
            IValuesService<Color> colorsService,
            ILotsService<Car> carsService,
            IUrlsService<Image> imagesService,
            IBaseService<Bid> bidsService
        )
        {
            Console.WriteLine("Loading database objects to memory...");

            this.ServicesDispatcher = new ServicesDispatcher()
                .InjectService<Make>(makesService)
                .InjectService<Model>(modelsService)
                .InjectService<Category>(categoriesService)
                .InjectService<Location>(locationsService)
                .InjectService<Currency>(currenciesService)
                .InjectService<Transmission>(transmissionsService)
                .InjectService<Fuel>(fuelsService)
                .InjectService<Color>(colorsService)
                .InjectService<Car>(carsService)
                .InjectService<Image>(imagesService);

            this.imagesService = imagesService;
            this.bidsService = bidsService;
        }

        private IServicesDispatcher ServicesDispatcher { get; set; }

        public void Run()
        {
            this.FetchDataFromJSONFiles(bidsPath, ParserType.Bid);
        }

        private void FetchDataFromJSONFiles(string path, ParserType type)
        {
            IEnumerable<string> jsonFiles = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories);
            List<string> failedJsons = new List<string>();
            int fileIndex = 0;

            foreach (string jsonFile in jsonFiles)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(jsonFile))
                    {
                        string json = sr.ReadToEnd();

                        switch (type)
                        {
                            case ParserType.Car:
                                this.DispatchCarFromJSON(json);
                                break;
                            case ParserType.Bid:
                                this.DispatchBidsFromJSON(json);
                                break;
                            default:
                                throw new InvalidOperationException("Parser type not supported.");
                        }

                        Console.Clear();
                        Console.WriteLine("File: {0}", ++fileIndex);
                    }
                }
                catch (JsonException)
                {
                    failedJsons.Add(jsonFile);
                }
            }
        }

        private void FetchCarFromWeb(string lot)
        {
            if (this.ServicesDispatcher.EntityExists<Car>(lot))
            {
                return;
            }

            WebRequest carRequest = WebRequest.Create(string.Format(carUrl, lot));
            using (WebResponse carResponse = carRequest.GetResponse())
            {
                Stream carDataStream = carResponse.GetResponseStream();
                using (StreamReader carReader = new StreamReader(carDataStream))
                {
                    string carResponseJSON = carReader.ReadToEnd();
                    this.DispatchCarFromJSON(carResponseJSON);
                }
            }
        }

        private void FetchLotImagesFromWeb(string lotNumber, string carJSON, int carId)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(imagesUrl, lotNumber));
            request.ContentType = "application/json";
            request.Method = "POST";

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(carJSON);
                writer.Flush();
                writer.Close();
            }

            using (WebResponse response = request.GetResponse())
            {
                Stream dataStream = ((HttpWebResponse)response).GetResponseStream();
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    string responseJSON = reader.ReadToEnd();
                    dynamic deserializedResponse = JsonConvert.DeserializeObject(responseJSON);
                    dynamic images = deserializedResponse.data.imagesList.FULL_IMAGE;

                    foreach (dynamic imageJSON in images)
                    {
                        string url = imageJSON.url.ToString();

                        Image image = new Image
                        {
                            Url = url,
                            CarId = carId
                        };

                        //this.ServicesDispatcher.AddEntity<Image>(image, url);
                        this.imagesService.Add(image);
                    }
                }
            }
        }

        private void DispatchCarFromJSON(string json)
        {
            dynamic carDeserializedResponse = JsonConvert.DeserializeObject(json);
            dynamic lotDetails = carDeserializedResponse.data.lotDetails;

            string lot = lotDetails.ln;

            if (this.ServicesDispatcher.EntityExists<Car>(lot))
            {
                return;
            }

            int year = lotDetails.lcy;
            string title = lotDetails.ld ?? string.Empty;
            string vin = lotDetails.fv ?? string.Empty;
            int estimateValue = lotDetails.la;
            int odometer = lotDetails.orr;

            int engine = 0;
            int.TryParse(Regex.Match((lotDetails.egn ?? string.Empty).ToString(), @"\d+").Value, out engine);

            string primaryDamage = lotDetails.dd ?? string.Empty;
            string secondaryDamage = lotDetails.sdd ?? string.Empty;
            string bodyStyle = lotDetails.bstl ?? string.Empty;
            string drive = lotDetails.drv ?? string.Empty;

            double auctionTimestamp = 0;
            double.TryParse((lotDetails.ad ?? string.Empty).ToString(), out auctionTimestamp);
            DateTime auctionOn = UnixTimeStampToDateTime(auctionTimestamp);

            Make make = this.ServicesDispatcher.GetEntity<Make>((lotDetails.mkn ?? string.Empty).ToString());
            Model model = this.ServicesDispatcher.GetEntity<Model>((lotDetails.lm ?? string.Empty).ToString());
            Category category = this.ServicesDispatcher.GetEntity<Category>((lotDetails.td ?? string.Empty).ToString());
            Location location = this.ServicesDispatcher.GetEntity<Location>((lotDetails.yn ?? string.Empty).ToString());
            Currency currency = this.ServicesDispatcher.GetEntity<Currency>((lotDetails.cuc ?? string.Empty).ToString());
            Transmission transmission = this.ServicesDispatcher.GetEntity<Transmission>((lotDetails.tsmn ?? string.Empty).ToString());
            Fuel fuel = this.ServicesDispatcher.GetEntity<Fuel>((lotDetails.ftd ?? string.Empty).ToString());
            Color color = this.ServicesDispatcher.GetEntity<Color>((lotDetails.clr ?? string.Empty).ToString());

            Car car = new Car
            {
                MakeId = make.Id,
                ModelId = model.Id,
                CategoryId = category.Id,
                LocationId = location.Id,
                CurrencyId = currency.Id,
                TransmissionId = transmission.Id,
                FuelId = fuel.Id,
                ColorId = color.Id,
                Lot = lot,
                Year = year,
                Title = title,
                VIN = vin,
                EstimateValue = estimateValue,
                Odometer = odometer,
                Engine = engine,
                PrimaryDamage = primaryDamage,
                SecondaryDamage = secondaryDamage,
                BodyStyle = bodyStyle,
                Drive = drive,
                AuctionOn = auctionOn
            };

            this.ServicesDispatcher.AddEntity(car, lot);

            // fetch images
            string carJSON = JsonConvert.SerializeObject(lotDetails);
            this.FetchLotImagesFromWeb(lot, carJSON, car.Id);
        }

        private void DispatchBidsFromJSON(string json)
        {
            dynamic bidsDeserializedResponse = JsonConvert.DeserializeObject(json);

            foreach (dynamic bidObject in bidsDeserializedResponse)
            {
                int lotNumber;
                int.TryParse(Regex.Match((bidObject.lot ?? string.Empty).ToString(), @"\d+").Value, out lotNumber);
                string lot = lotNumber.ToString();

                dynamic dirtyBids = bidObject.bids;
                List<string> bids = new List<string>();

                if (!this.ServicesDispatcher.EntityExists<Car>(lot))
                {
                    this.FetchCarFromWeb(lot);
                }

                int carId = this.ServicesDispatcher.GetEntityId<Car>(lot);

                foreach (string bid in dirtyBids)
                {
                    if (bid.Contains("£"))
                    {
                        int cost;
                        int.TryParse(Regex.Match(bid, @"\d+").Value, out cost);

                        Bid newBid = new Bid
                        {
                            Cost = cost,
                            CarId = carId
                        };

                        this.bidsService.Add(newBid);
                    }
                }
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTime)
        {
            if (unixTime > 0)
            {
                DateTime unixEpoch = DateTime.ParseExact("1970-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime convertedTime = unixEpoch.AddMilliseconds(unixTime);
                return convertedTime;
            }

            return DateTime.Now;
        }
    }
}