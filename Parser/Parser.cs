namespace Parser
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    public class Parser
    {
        public Car[] Cars { get; private set; }
        public string Path { get; private set; }

        public void Execute(string path)
        {
            this.Path = path;
            this.FetchCars();
            this.NormalizeCars(this.Cars);
        }

        private void FetchCars()
        {
            string[] jsonFiles = Directory.GetFiles(this.Path, "*.json");
            List<Car> cars = new List<Car>();

            foreach (string jsonFile in jsonFiles)
            {
                using (StreamReader sr = new StreamReader(jsonFile))
                {
                    string json = sr.ReadToEnd();
                    Car[] newCars = JsonConvert.DeserializeObject<Car[]>(json);
                    cars.AddRange(newCars);
                }
            }

            this.Cars = cars.ToArray();
        }

        private void NormalizeCars(Car[] cars)
        {
            foreach (Car car in cars)
            {
                car.Images.RemoveWhere(x => x == null);
                car.Bids.RemoveWhere(x => x[0] != '£');
                car.Lot = Regex.Match(car.Lot, @"\d+").Value;

                string[] splitInfo = car.Model.Split(' ');

                if (splitInfo.Length == 2)
                {
                    continue;
                    car.Year = int.Parse(splitInfo[0]);
                    car.Make = splitInfo[1].Trim();
                    car.Model = car.Make;
                }
                else
                {
                    car.Year = int.Parse(splitInfo[0]);

                    if (splitInfo.Length == 3)
                    {
                        car.Make = splitInfo[1];

                        car.Model = string.Empty;
                        for (int i = 2; i < splitInfo.Length; i++)
                        {
                            car.Model += string.Format("{0} ", splitInfo[i]);
                        }
                        
                    }
                    else
                    {
                        car.Make = splitInfo[2];

                        car.Model = string.Empty;
                        for (int i = 3; i < splitInfo.Length; i++)
                        {
                            car.Model += string.Format("{0} ", splitInfo[i]);
                        }
                        car.Model = car.Model.Trim();
                    }
                }

                if (car.Make != null && car.Model != null && car.Year != 0)
                {
                    car.IsValid = true;
                }
                else
                {
                    car.IsValid = false;
                }
            }

            this.Cars = cars;
        }
    }
}