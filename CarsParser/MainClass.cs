namespace CarsParser
{
    using Autofac;

    public class MainClass
    {
        public static void Main()
        {
            IContainer container = ContainerConfig.Configure();

            using (ILifetimeScope scope = container.BeginLifetimeScope())
            {
                IApplication application = scope.Resolve<IApplication>();
                application.Run();
            } 
        }
    }
}