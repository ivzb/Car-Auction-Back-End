namespace Parser
{
    using Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class DbInflater
    {
        public DbInflater()
        {
            this.Context = new CopartEntities();
        }

        public Car[] Cars { get; private set; }
        private CopartEntities Context { get; set; }

        public void Execute(Car[] cars)
        {
            this.Cars = cars;

            List<Category> categories = this.Context.Categories.ToList();
            List<Make> makes = this.Context.Makes.ToList();
            List<Model> models = this.Context.Models.ToList();

            int counter = 1;
            foreach (Car car in cars)
            {
                Console.Clear();
                Console.WriteLine("{0}/{1}", counter++, cars.Length);

                if (!car.IsValid) continue;

                Category carCategory = categories.FirstOrDefault(x => x.Value == car.Category);

                if (carCategory == null)
                {
                    carCategory = new Category
                    {
                        Value = car.Category
                    };

                    this.Context.Categories.Add(carCategory);
                    this.Context.SaveChanges();
                    categories.Add(carCategory);
                }

                Make carMake = makes.FirstOrDefault(x => x.Value == car.Make);

                if (carMake == null)
                {
                    carMake = new Make
                    {
                        Value = car.Make
                    };

                    this.Context.Makes.Add(carMake);
                    this.Context.SaveChanges();
                    makes.Add(carMake);
                }

                Model carModel = models.FirstOrDefault(x => x.Value == car.Model);

                if (carModel == null)
                {
                    carModel = new Model
                    {
                        Value = car.Model
                    };

                    this.Context.Models.Add(carModel);
                    this.Context.SaveChanges();
                    models.Add(carModel);
                }

                try
                {
                    Data.Car newCar = new Data.Car
                    {
                        Lot = int.Parse(car.Lot),
                        Year = car.Year,
                        MakeId = carMake.Id,
                        ModelId = carModel.Id,
                        CategoryId = carCategory.Id,
                        Damage = car.Damage,
                        EngineType = car.EngineType,
                        CreatedOn = DateTime.Now
                    };

                    this.Context.Cars.Add(newCar);
                    this.Context.SaveChanges();

                    foreach (string image in car.Images)
                    {
                        Image carImage = new Image
                        {
                            Url = image,
                            CarId = newCar.Id
                        };

                        this.Context.Images.Add(carImage);
                    }

                    foreach (string bid in car.Bids)
                    {
                        string[] splitBid = bid.Split(' ');
                        int bidValue = int.Parse(Regex.Match(splitBid[0], @"\d+").Value);

                        string bidInfo = string.Empty;
                        for (int i = 1; i < splitBid.Length; i++)
                        {
                            bidInfo += string.Format("{0} ", splitBid[i]);
                        }
                        bidInfo = bidInfo.Trim();

                        Bid carBid = new Bid
                        {
                            Value = bidValue,
                            Location = bidInfo,
                            CarId = newCar.Id
                        };

                        this.Context.Bids.Add(carBid);
                    }

                    this.Context.SaveChanges();
                }
                catch (Exception) { }
            }

            //this.FetchCars();
            //this.NormalizeCars(this.Cars);
        }
    }
}
