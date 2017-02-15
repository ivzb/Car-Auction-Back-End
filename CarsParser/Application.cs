using Data;
using Services.Interfaces;
using System;
using System.Linq;

namespace CarsParser
{
    public class Application : IApplication
    {
        private readonly IDefaultService<Make> makesService;

        public Application(IDefaultService<Make> makesService)
        {
            this.makesService = makesService;
        }

        public void Run()
        {
            Console.WriteLine(makesService.Get().Count());
        }
    }
}
