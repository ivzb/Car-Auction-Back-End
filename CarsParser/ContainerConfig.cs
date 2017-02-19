namespace CarsParser
{
    using Autofac;
    using Data;
    using Data.Common;
    using Services;
    using Services.Interfaces;
    using System.Data.Entity;
    using System.Reflection;

    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder
                .RegisterType<Application>()
                .As<IApplication>();

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

            return builder.Build();
        }
    }
}