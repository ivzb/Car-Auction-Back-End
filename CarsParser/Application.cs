namespace CarsParser
{
    using Data;
    using Data.Common.Models;
    using Newtonsoft.Json;
    using Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;

    public class Application : IApplication
    {
        private const string carUrl = "https://www.copart.co.uk/public/data/lotdetails/solr/{0}";
        private const string imagesUrl = "https://www.copart.co.uk/public/data/lotdetails/solr/lotImages/{0}";

        private readonly IDefaultService<Car> carsService;

        public Application(
            IDefaultService<Make> makesService,
            IDefaultService<Model> modelsService,
            IDefaultService<Category> categoriesService,
            IDefaultService<Location> locationsService,
            IDefaultService<Currency> currenciesService,
            IDefaultService<Transmission> transmissionsService,
            IDefaultService<Fuel> fuelsService,
            IDefaultService<Color> colorsService,
            IDefaultService<Car> carsService
        ) {
            this.ServicesDispatcher = new ServicesDispatcher();
            this.ServicesDispatcher.InjectService<Make>(makesService);
            this.ServicesDispatcher.InjectService<Model>(modelsService);
            this.ServicesDispatcher.InjectService<Category>(categoriesService);
            this.ServicesDispatcher.InjectService<Location>(locationsService);
            this.ServicesDispatcher.InjectService<Currency>(currenciesService);
            this.ServicesDispatcher.InjectService<Transmission>(transmissionsService);
            this.ServicesDispatcher.InjectService<Fuel>(fuelsService);
            this.ServicesDispatcher.InjectService<Color>(colorsService);

            this.carsService = carsService;
        }

        private IServicesDispatcher ServicesDispatcher { get; set; }

        public void Run()
        {
            this.FetchCarForLot(18866507);
        }

        private void FetchCarForLot(int lot)

        {
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

                    this.carsService.Add(car);

                    // fetch images
                    string carJSON = JsonConvert.SerializeObject(car);
                    FetchImagesForLot(lot, carJSON);
                }
            }
        }

        private static void FetchImagesForLot(int lotNumber, string carJSON)
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