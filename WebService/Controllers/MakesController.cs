﻿namespace WebService.Controllers
{
    using Data;
    using Services.Interfaces;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.OData;
    using System.Web.Http.OData.Query;
    using WebService.Controllers.Base;
    
    public class MakesController : BaseController<Make>
    {
        public MakesController(IBaseService<Make> service)
            : base(service)
        {
        }

        // GET: odata/Makes
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.None,
            AllowedFunctions = AllowedFunctions.None,
            AllowedLogicalOperators = AllowedLogicalOperators.Equal |
                                      AllowedLogicalOperators.NotEqual,
            AllowedOrderByProperties = "ImageUrl, Value",
            AllowedQueryOptions = AllowedQueryOptions.Expand |
                                         AllowedQueryOptions.Top |
                                         AllowedQueryOptions.Skip |
                                         AllowedQueryOptions.OrderBy |
                                         AllowedQueryOptions.Filter)]
        public IQueryable<Make> GetMakes()
        {
            return base.Get();
        }

        // GET: odata/Makes(5)
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.None,
            AllowedFunctions = AllowedFunctions.None,
            AllowedLogicalOperators = AllowedLogicalOperators.None,
            AllowedQueryOptions = AllowedQueryOptions.None)]
        public SingleResult<Make> GetMake([FromODataUri] int key)
        {
            return base.Get(key);
        }

        // GET: odata/Makes(5)/Models
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.None,
            AllowedFunctions = AllowedFunctions.None,
            AllowedLogicalOperators = AllowedLogicalOperators.None,
            AllowedOrderByProperties = "Value",
            AllowedQueryOptions = AllowedQueryOptions.OrderBy)]
        public IQueryable<Model> GetModels([FromODataUri] int key)
        {
            return base.Get<Model>(key, x => x.Models);
        }
    }
}