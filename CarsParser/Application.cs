namespace CarsParser
{
    using Data;
    using Newtonsoft.Json;
    using Services.Interfaces;
    using System;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;

    public class Application : IApplication
    {
        private const string carUrl = "https://www.copart.co.uk/public/data/lotdetails/solr/{0}";
        private const string imagesUrl = "https://www.copart.co.uk/public/data/lotdetails/solr/lotImages/{0}";

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
            IUrlsService<Image> imagesService
        ) {
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
        }

        private IServicesDispatcher ServicesDispatcher { get; set; }

        public void Run()
        {
#if DEBUG
            this.FetchCarForLot("19410837");
#elif RELEASE
            try
            {
                this.FetchCarForLot("19410837");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                // todo: save failed lots in db and examine them later
            }
#endif
        }

        private void FetchCarForLot(string lot)
        {
            if (this.ServicesDispatcher.EntityExists<Car>(lot))
            {
                throw new ArgumentException(string.Format("Car with lot #{0} already exists.", lot));
            }
            
            WebRequest carRequest = WebRequest.Create(string.Format(carUrl, lot));
            using (WebResponse carResponse = carRequest.GetResponse())
            {
                Stream carDataStream = carResponse.GetResponseStream();
                using (StreamReader carReader = new StreamReader(carDataStream))
                {
                    string carResponseJSON = carReader.ReadToEnd();
                    dynamic carDeserializedResponse = JsonConvert.DeserializeObject(carResponseJSON);
                    dynamic lotDetails = carDeserializedResponse.data.lotDetails;

                    Make make = this.ServicesDispatcher.GetEntity<Make>(lotDetails.mkn.ToString());
                    Model model = this.ServicesDispatcher.GetEntity<Model>(lotDetails.lm.ToString());
                    Category category = this.ServicesDispatcher.GetEntity<Category>(lotDetails.td.ToString());
                    Location location = this.ServicesDispatcher.GetEntity<Location>(lotDetails.yn.ToString());
                    Currency currency = this.ServicesDispatcher.GetEntity<Currency>(lotDetails.cuc.ToString());
                    Transmission transmission = this.ServicesDispatcher.GetEntity<Transmission>(lotDetails.tsmn.ToString());
                    Fuel fuel = this.ServicesDispatcher.GetEntity<Fuel>(lotDetails.ftd.ToString());
                    Color color = this.ServicesDispatcher.GetEntity<Color>(lotDetails.clr.ToString());

                    int year = lotDetails.lcy;
                    string title = lotDetails.ld;
                    string vin = lotDetails.fv;
                    int estimateValue = lotDetails.la;
                    int odometer = lotDetails.orr;
                    int engine = int.Parse(Regex.Match(lotDetails.egn.ToString(), @"\d+").Value);
                    string primaryDamage = lotDetails.dd;
                    string secondaryDamage = lotDetails.sdd;
                    string bodyStyle = lotDetails.bstl;
                    string drive = lotDetails.drv;
                    DateTime auctionOn = UnixTimeStampToDateTime((double)lotDetails.ad);

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

                    this.ServicesDispatcher.AddEntity(car, title);

                    // fetch images
                    string carJSON = JsonConvert.SerializeObject(lotDetails);
                    this.FetchImagesForLot(lot, carJSON, car.Id);
                }
            }
        }

        private void FetchImagesForLot(string lotNumber, string carJSON, int carId)
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

                    foreach(dynamic imageJSON in images)
                    {
                        string url = imageJSON.url.ToString();

                        Image image = new Image
                        {
                            Url = url,
                            CarId = carId
                        };

                        this.ServicesDispatcher.AddEntity<Image>(image, url);
                    }
                }
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTime)
        {
            DateTime unixEpoch = DateTime.ParseExact("1970-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime convertedTime = unixEpoch.AddMilliseconds(unixTime);
            return convertedTime;
        }
    }
}