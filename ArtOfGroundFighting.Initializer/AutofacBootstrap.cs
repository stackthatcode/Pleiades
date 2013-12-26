using Autofac;
using Pleiades.App.Injection;

namespace ArtOfGroundFighting.Initializer
{
    public class AutofacBootstrap
    {
        public static IContainerAdapter CreateContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<InitializerModules>();
            var container = builder.Build();
            var containerAdapter = new AutofacContainer(container.BeginLifetimeScope());
            return containerAdapter;
        }
    }
}
