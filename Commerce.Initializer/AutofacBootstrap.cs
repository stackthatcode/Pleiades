using Autofac;
using Pleiades.Application.Injection;
using Pleiades.Web.Autofac;

namespace Commerce.Initializer
{
    public class AutofacBootstrap
    {
        public static IContainerAdapter CreateContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommerceInitializerModules>();
            var container = builder.Build();
            var containerAdapter = new AutofacContainer(container.BeginLifetimeScope());
            return containerAdapter;
        }
    }
}
