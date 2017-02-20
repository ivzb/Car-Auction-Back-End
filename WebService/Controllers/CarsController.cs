namespace WebService.Controllers
{
    using Data;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.OData;
    using WebService.Controllers.Base;

    //public class CarsController : GenericController<Car, CarViewModel>
    //{
    //    public CarsController()
    //        : base()
    //    {
    //    }

    //    // GET: odata/Cars
    //    [EnableQuery]
    //    public IQueryable<Car> GetCars()
    //    {
    //        return base.Context.Cars;
    //    }

    //    // GET: odata/Cars(5)
    //    [EnableQuery]
    //    public SingleResult<Car> GetCar([FromODataUri] int key)
    //    {
    //        return SingleResult.Create(base.Context.Cars.Where(car => car.Id == key));
    //    }

    //    // GET: odata/Cars(5)/Bids
    //    [EnableQuery]
    //    public IQueryable<Bid> GetBids([FromODataUri] int key)
    //    {
    //        return base.Context.Cars.Where(m => m.Id == key).SelectMany(m => m.Bids);
    //    }

    //    // GET: odata/Cars(5)/Category
    //    [EnableQuery]
    //    public SingleResult<Category> GetCategory([FromODataUri] int key)
    //    {
    //        return SingleResult.Create(base.Context.Cars.Where(m => m.Id == key).Select(m => m.Category));
    //    }

    //    // GET: odata/Cars(5)/Make
    //    [EnableQuery]
    //    public SingleResult<Make> GetMake([FromODataUri] int key)
    //    {
    //        return SingleResult.Create(base.Context.Cars.Where(m => m.Id == key).Select(m => m.Make));
    //    }

    //    // GET: odata/Cars(5)/Model
    //    [EnableQuery]
    //    public SingleResult<Model> GetModel([FromODataUri] int key)
    //    {
    //        return SingleResult.Create(base.Context.Cars.Where(m => m.Id == key).Select(m => m.Model));
    //    }

    //    // GET: odata/Cars(5)/Images
    //    [EnableQuery]
    //    public IQueryable<Image> GetImages([FromODataUri] int key)
    //    {
    //        return base.Context.Cars.Where(m => m.Id == key).SelectMany(m => m.Images);
    //    }
    //}
}