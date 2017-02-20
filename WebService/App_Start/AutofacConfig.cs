namespace WebService
{
    using Autofac;
    using Autofac.Integration.WebApi;
    using Data;
    using Data.Common;
    using Services;
    using Services.Interfaces;
    using System.Data.Entity;
    using System.Reflection;
    using System.Web.Http;

    public static class AutofacConfig
    {
        public static void RegisterAutofac()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            // Register your MVC controllers.
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterWebApiModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiModelBinderProvider();

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterWebApiFilterProvider(config);

            // Register services
            RegisterServices(builder);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder
                .Register(x => new Entities())
                .As<DbContext>();

            builder
                .RegisterGeneric(typeof(DbRepository<>))
                .As(typeof(IDbRepository<>));

            builder
                .RegisterGeneric(typeof(DbRepository<,>))
                .As(typeof(IDbRepository<,>));

            builder
               .RegisterGeneric(typeof(BaseService<>))
               .As(typeof(IBaseService<>));

            builder
               .RegisterGeneric(typeof(ValuesService<>))
               .As(typeof(IValuesService<>));

            builder
               .RegisterGeneric(typeof(TitlesService<>))
               .As(typeof(ITitlesService<>));

            builder
               .RegisterGeneric(typeof(UrlsService<>))
               .As(typeof(IUrlsService<>));

            builder
               .RegisterGeneric(typeof(LotsService<>))
               .As(typeof(ILotsService<>));

            Assembly servicesAssembly = Assembly.GetAssembly(typeof(IGenericService));
            builder
                .RegisterAssemblyTypes(servicesAssembly)
                .AsImplementedInterfaces();
        }
    }
}