//using Autofac;
//using Data;
//using System;
//using System.Collections.Generic;

//namespace CarsParser
//{
//    public class Engine
//    {
//        public Engine()
//        {
//            //this.Makes = this.Context.Makes.ToDictionary(x => x.Value);

//            int lotNumber = 41385736;
//            FetchCarForLot(lotNumber);
//        }

//        private void RegisterAutofacServices(ContainerBuilder builder)
//        {
            
//        }

//        private Dictionary<string, Make> Makes { get; set; }

//        private Make GetMake(string make)
//        {
//            if (!this.Makes.ContainsKey(make))
//            {
//                Make newMake = new Make
//                {
//                    Value = make
//                };

//                this.Context.Makes.Add(newMake);
//                this.Context.SaveChanges();
//                this.Makes.Add(make, newMake); 
//            }

//            return this.Makes[make];
//        }

//        private static void FetchCarForLot(int lotNumber)
//        {
//            WebRequest carRequest = WebRequest.Create(string.Format("https://www.copart.co.uk/public/data/lotdetails/solr/{0}", lotNumber));
//            using (WebResponse carResponse = carRequest.GetResponse())
//            {
//                Stream carDataStream = carResponse.GetResponseStream();
//                using (StreamReader carReader = new StreamReader(carDataStream))
//                {
//                    string carResponseJSON = carReader.ReadToEnd();
//                    dynamic carDeserializedResponse = JsonConvert.DeserializeObject(carResponseJSON);
//                    dynamic car = carDeserializedResponse.data.lotDetails;

//                    string make = car.mkn;
//                    string model = car.lm;
//                    string category = car.td;
//                    string location = car.yn;
//                    string currency = car.cuc;
//                    string transmission = car.tsmn;
//                    string fuel = car.ftd;

//                    //int lotNumber = car.ln;
//                    int year = car.lcy;
//                    string title = car.ld;
//                    string vin = car.fv;
//                    int estimateValue = car.la;
//                    int odometer = car.orr;
//                    string engine = car.egn;
//                    string primaryDamage = car.dd;
//                    string secondaryDamage = car.sdd;
//                    string bodyStyle = car.bstl;
//                    string color = car.clr;
//                    string drive = car.drv;

//                    DateTime AuctionDate = UnixTimeStampToDateTime((double)car.ad);

//                    // fetch images
//                    string carJSON = JsonConvert.SerializeObject(car);
//                    FetchImagesForLot(lotNumber, carJSON);
//                }
//            }
//        }

//        private static void FetchImagesForLot(int lotNumber, string carJSON)
//        {
//            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.copart.co.uk/public/data/lotdetails/solr/lotImages/" + lotNumber);
//            request.ContentType = "application/json";
//            request.Method = "POST";

//            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
//            {
//                writer.Write(carJSON);
//                writer.Flush();
//                writer.Close();
//            }

//            using (WebResponse response = request.GetResponse())
//            {
//                Stream dataStream = ((HttpWebResponse)response).GetResponseStream();
//                using (StreamReader reader = new StreamReader(dataStream))
//                {
//                    string responseJSON = reader.ReadToEnd();
//                    dynamic deserializedResponse = JsonConvert.DeserializeObject(responseJSON);
//                    dynamic images = deserializedResponse.data.imagesList.FULL_IMAGE;
//                }
//            }
//        }

//        public static DateTime UnixTimeStampToDateTime(double unixTime)
//        {
//            DateTime unixEpoch = DateTime.ParseExact("1970-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
//            DateTime convertedTime = unixEpoch.AddMilliseconds(unixTime);
//            return convertedTime;
//        }
//    }
//}
