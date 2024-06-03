using Autofac;
using Services.Implementations;
using Services.Interfaces;

namespace WebApi.ApplicationConfiguration
{
    public class ApiDiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //autofac register transient default lifetime
            RegisterServices(builder);
            RegisterSettings(builder);            
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            var servicesAssembly = typeof(Services.IServiceRegistrator).Assembly;
            builder.RegisterAssemblyTypes(servicesAssembly)
                .Where(t => t.GetInterfaces().Contains(typeof(Services.IServiceRegistrator)))
                .AsImplementedInterfaces();
        }

        private void RegisterSettings(ContainerBuilder builder)
        {
            builder.RegisterType<GlobalSettings>().As<IGlobalSettings>().SingleInstance();
        }
    }
}
