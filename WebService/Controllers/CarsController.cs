namespace WebService.Controllers
{
    using Data;
    using Services.Interfaces;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using WebService.Controllers.Base;

    public class CarsController : BaseController<Car>
    {
        public CarsController(IBaseService<Car> service)
            : base(service)
        {
        }

        // GET: odata/Cars
        [EnableQuery(
            PageSize = 21,
            AllowedArithmeticOperators = AllowedArithmeticOperators.None,
            AllowedFunctions           = AllowedFunctions.None,
            AllowedLogicalOperators    = AllowedLogicalOperators.None,
            AllowedOrderByProperties   = "AuctionOn",
            AllowedQueryOptions        = AllowedQueryOptions.Expand  |
                                         AllowedQueryOptions.Top     |
                                         AllowedQueryOptions.Skip    |
                                         AllowedQueryOptions.OrderBy)]
        public IQueryable<Car> GetCars()
        {
            return base.Get();
        }

        // GET: odata/Cars(5)
        [EnableQuery(
            AllowedQueryOptions = AllowedQueryOptions.Expand,
            AllowedFunctions    = AllowedFunctions.None
        )]
        public SingleResult<Car> GetCar([FromODataUri] int key)
        {
            return base.Get(key);
        }

        // GET: odata/Cars(5)/Bids
        [EnableQuery]
        public IQueryable<Bid> GetBids([FromODataUri] int key)
        {
            return base.Get<Bid>(key, x => x.Bids);
        }

        // GET: odata/Cars(5)/Category
        [EnableQuery]
        public SingleResult<Category> GetCategory([FromODataUri] int key)
        {
            return base.Get<Category>(key, x => x.Category);
        }

        // GET: odata/Cars(5)/Color
        [EnableQuery]
        public SingleResult<Color> GetColor([FromODataUri] int key)
        {
            return base.Get<Color>(key, x => x.Color);
        }

        // GET: odata/Cars(5)/Currency
        [EnableQuery]
        public SingleResult<Currency> GetCurrency([FromODataUri] int key)
        {
            return base.Get<Currency>(key, x => x.Currency);
        }

        // GET: odata/Cars(5)/Fuel
        [EnableQuery]
        public SingleResult<Fuel> GetFuel([FromODataUri] int key)
        {
            return base.Get<Fuel>(key, x => x.Fuel);
        }

        // GET: odata/Cars(5)/Location
        [EnableQuery]
        public SingleResult<Location> GetLocation([FromODataUri] int key)
        {
            return base.Get<Location>(key, x => x.Location);
        }

        // GET: odata/Cars(5)/Make
        [EnableQuery]
        public SingleResult<Make> GetMake([FromODataUri] int key)
        {
            return base.Get<Make>(key, x => x.Make);
        }

        // GET: odata/Cars(5)/Model
        [EnableQuery]
        public SingleResult<Model> GetModel([FromODataUri] int key)
        {
            return base.Get<Model>(key, x => x.Model);
        }

        // GET: odata/Cars(5)/Transmission
        [EnableQuery]
        public SingleResult<Transmission> GetTransmission([FromODataUri] int key)
        {
            return base.Get<Transmission>(key, x => x.Transmission);
        }

        // GET: odata/Cars(5)/Images
        [EnableQuery]
        public IQueryable<Image> GetImages([FromODataUri] int key)
        {
            return base.Get<Image>(key, x => x.Images);
        }
    }
}