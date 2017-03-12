namespace WebService.Controllers
{
    using Data;
    using Services.Interfaces;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using WebService.Controllers.Base;

    public class ModelsController : BaseController<Model>
    {
        public ModelsController(IBaseService<Model> service)
            : base(service)
        {
        }

        // GET: odata/Models(5)
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.None,
            AllowedFunctions = AllowedFunctions.None,
            AllowedLogicalOperators = AllowedLogicalOperators.None,
            AllowedQueryOptions = AllowedQueryOptions.None)]
        public SingleResult<Model> GetModel([FromODataUri] int key)
        {
            return base.Get(key);
        }

        // GET: odata/Models(5)/Cars
        [EnableQuery(
            PageSize = 21,
            AllowedArithmeticOperators = AllowedArithmeticOperators.None,
            AllowedFunctions = AllowedFunctions.None,
            AllowedLogicalOperators = AllowedLogicalOperators.None,
            AllowedOrderByProperties = "AuctionOn",
            AllowedQueryOptions = AllowedQueryOptions.Expand |
                                  AllowedQueryOptions.Top |
                                  AllowedQueryOptions.Skip |
                                  AllowedQueryOptions.OrderBy)]
        public IQueryable<Car> GetCars([FromODataUri] int key)
        {
            return base.Get<Car>(key, x => x.Cars);
        }
    }
}