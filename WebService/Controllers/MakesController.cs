namespace WebService.Controllers
{
    using Data;
    using Data.ViewModels;
    using Services.Interfaces;
    using System.Linq;
    using System.Web.Http.OData;
    using WebService.Controllers.Base;

    public class MakesController : GenericController<Make, MakeViewModel>
    {
        public MakesController(IBaseService<Make> service)
            : base(service)
        {
        }

        // GET: odata/Makes
        [EnableQuery]
        public IQueryable<Make> GetMakes()
        {
            return this.service.Get();
        }

        // GET: odata/Makes(5)
        [EnableQuery]
        public Make GetMake([FromODataUri] int key)
        {
            Make make = this.service.Get(key);
            return make;
        }

        // GET: odata/Makes(5)/Cars
        [EnableQuery]
        public IQueryable<Car> GetCars([FromODataUri] int key)
        {
            return this.service.Get().Where(m => m.Id == key).SelectMany(m => m.Cars);
        }
    }
}