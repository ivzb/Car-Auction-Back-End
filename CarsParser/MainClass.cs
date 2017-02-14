using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsParser
{
    public class MainClass
    {
        private const string CarsPath = @"C:\Users\izahariev\Documents\twork\daniauto data\cars\February\14th.json"; 
        public static void Main()
        {
            using (StreamReader sr = new StreamReader(CarsPath))
            {
                string carsJson = sr.ReadToEnd();
                dynamic jsonResponse = JsonConvert.DeserializeObject(carsJson);
                double a = Math.Abs(5);
            }
        }
    }
}
