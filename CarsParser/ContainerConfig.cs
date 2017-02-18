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
               .RegisterGeneric(typeof(DefaultService<>))
               .As(typeof(IDefaultService<>));

            Assembly servicesAssembly = Assembly.GetAssembly(typeof(INomenclatureService));
            builder
                .RegisterAssemblyTypes(servicesAssembly)
                .AsImplementedInterfaces();

            return builder.Build();
        }
    }
}