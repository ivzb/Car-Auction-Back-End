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
            var builder = new ContainerBuilder();

            builder.RegisterType<Application>()
                .As<IApplication>();

            builder.Register(x => new daniautoEntities())
                .As<DbContext>();
                //.InstancePerRequest();

            builder.RegisterGeneric(typeof(DbRepository<>))
                .As(typeof(IDbRepository<>));
                //.InstancePerRequest();

            builder.RegisterGeneric(typeof(DbRepository<,>))
                .As(typeof(IDbRepository<,>));
                //.InstancePerRequest();

            builder.RegisterGeneric(typeof(DefaultService<>))
               .As(typeof(IDefaultService<>));
               //.InstancePerRequest();

            var servicesAssembly = Assembly.GetAssembly(typeof(INomenclatureService));
            builder.RegisterAssemblyTypes(servicesAssembly).AsImplementedInterfaces();

            //builder.RegisterType<MakesService>()
                //.As<IMakesService>();

            return builder.Build();
        }
    }
}