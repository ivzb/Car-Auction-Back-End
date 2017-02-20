namespace WebService.Controllers.Base
{
    using AutoMapper;
    using System.Web.Http;
    using System.Web.Http.OData;
    using WebService.Infrastructure.Automapper;

    public class BaseController : ODataController
    {
        protected IMapper Mapper
        {
            get
            {
                return AutoMapperConfig.Configuration.CreateMapper();
            }
        }
    }
}