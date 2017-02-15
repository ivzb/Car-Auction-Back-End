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
        private const string BidsPath = @"C:\Users\izahariev\Documents\twork\daniauto data\bids\February\15th"; 
        public static void Main(string[] args)
        {
            Parser parser = new Parser();
            parser.Execute(BidsPath);
            Car[] cars = parser.Cars;

            DbInflater dbInflater = new DbInflater();
            dbInflater.Execute(cars);
        }
    }
}