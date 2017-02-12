namespace Parser
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Linq;
    using Data;

    public class MainClass
    {
        private const string CarsPath = @"C:\Users\izahariev\Desktop\cars\2-9-17"; 
        public static void Main(string[] args)
        {
            Parser parser = new Parser();
            parser.Execute(CarsPath);
            Car[] cars = parser.Cars;

            DbInflater dbInflater = new DbInflater();
            dbInflater.Execute(cars);
        }
    }
}