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

    public class Application : IApplication
    {

        private const string carUrl = "https://www.copart.co.uk/public/data/lotdetails/solr/{0}";
        private const string imagesUrl = "https://www.copart.co.uk/public/data/lotdetails/solr/lotImages/{0}";

        public Application(
            IDefaultService<Make> makesService,
            IDefaultService<Model> modelService,
            IDefaultService<Category> categoryService,
            IDefaultService<Location> locationService,
            IDefaultService<Currency> currencyService,
            IDefaultService<Transmission> transmissionService,
            IDefaultService<Fuel> fuelService,
            IDefaultService<Color> colorService
        ) {
            this.ServicesDispatcher = new ServicesDispatcher();
            this.ServicesDispatcher.InjectService<Make>(makesService);
            this.ServicesDispatcher.InjectService<Model>(modelService);
            this.ServicesDispatcher.InjectService<Category>(categoryService);
            this.ServicesDispatcher.InjectService<Location>(locationService);
            this.ServicesDispatcher.InjectService<Currency>(currencyService);
            this.ServicesDispatcher.InjectService<Transmission>(transmissionService);
            this.ServicesDispatcher.InjectService<Fuel>(fuelService);
            this.ServicesDispatcher.InjectService<Color>(colorService);
        }

        private IServicesDispatcher ServicesDispatcher { get; set; }

        public void Run()
        {
            Make test = this.ServicesDispatcher.GetEntity<Make>("test123");
            Console.WriteLine(test.Value);
        }

        private void FetchCarForLot(int lotNumber)
        {
            WebRequest carRequest = WebRequest.Create(string.Format(carUrl, lotNumber));
            using (WebResponse carResponse = carRequest.GetResponse())
            {
                Stream carDataStream = carResponse.GetResponseStream();
                using (StreamReader carReader = new StreamReader(carDataStream))
                {
                    string carResponseJSON = carReader.ReadToEnd();
                    dynamic carDeserializedResponse = JsonConvert.DeserializeObject(carResponseJSON);
                    dynamic car = carDeserializedResponse.data.lotDetails;

                    Make make = this.ServicesDispatcher.GetEntity<Make>(car.mkn);
                    Model model = this.ServicesDispatcher.GetEntity<Model>(car.lm);
                    Category category = this.ServicesDispatcher.GetEntity<Category>(car.td);
                    Location location = this.ServicesDispatcher.GetEntity<Location>(car.yn);
                    Currency currency = this.ServicesDispatcher.GetEntity<Currency>(car.cuc);
                    Transmission transmission = this.ServicesDispatcher.GetEntity<Transmission>(car.tsmn);
                    Fuel fuel = this.ServicesDispatcher.GetEntity<Fuel>(car.ftd);
                    Color color = this.ServicesDispatcher.GetEntity<Color>(car.clr);

                    // todo: inspect if all types here and in the DB are the same
                    // there should be no differences!
                    //int lotNumber = car.ln;
                    int year = car.lcy;
                    string title = car.ld;
                    string vin = car.fv;
                    int estimateValue = car.la;
                    int odometer = car.orr;
                    string engine = car.egn;
                    string primaryDamage = car.dd;
                    string secondaryDamage = car.sdd;
                    string bodyStyle = car.bstl;
                    string drive = car.drv;

                    DateTime AuctionDate = UnixTimeStampToDateTime((double)car.ad);

                    // fetch images
                    string carJSON = JsonConvert.SerializeObject(car);
                    FetchImagesForLot(lotNumber, carJSON);
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