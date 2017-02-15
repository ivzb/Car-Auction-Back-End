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
        private readonly IDefaultService<Make> makesService;

        private Dictionary<string, Make> Makes { get; set; }

        private const string carUrl = "https://www.copart.co.uk/public/data/lotdetails/solr/{0}";
        private const string imagesUrl = "https://www.copart.co.uk/public/data/lotdetails/solr/lotImages/{0}";

        public Application(IDefaultService<Make> makesService)
        {
            this.makesService = makesService;
        }

        public void Run()
        {
            this.Makes = this.makesService.Get().ToDictionary(x => x.Value);

            Make test = this.GetMake("test");
        }

        private Make GetMake(string key)
        {
            Make make;
            bool makeFound = this.Makes.TryGetValue(key, out make);

            if (!makeFound)
            {
                make = new Make
                {
                    Value = key
                };

                this.makesService.Add(make);
                this.Makes.Add(key, make);
            }

            return make;
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

                    Make make = this.GetMake(car.mkn);
                    string model = car.lm;
                    string category = car.td;
                    string location = car.yn;
                    string currency = car.cuc;
                    string transmission = car.tsmn;
                    string fuel = car.ftd;

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
                    string color = car.clr;
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