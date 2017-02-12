namespace WebService
{
    using System.Web.Http;
    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using WebService.Data;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // odata API routes
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Car>("Cars");
            builder.EntitySet<Bid>("Bids");
            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Make>("Makes");
            builder.EntitySet<Model>("Models");
            builder.EntitySet<Image>("Images");
            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());

            // default route
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
