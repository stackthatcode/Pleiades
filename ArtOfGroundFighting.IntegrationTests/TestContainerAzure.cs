using Autofac;
using Commerce.Application.Azure;
using ArtOfGroundFighting.Initializer;

namespace ArtOfGroundFighting.IntegrationTests
{
    public class TestContainerAzure
    {
        static readonly ILifetimeScope RootScope = null;

        public static ILifetimeScope LifetimeScope()
        {
            return RootScope.BeginLifetimeScope();
        }
         
        static TestContainerAzure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommerceInitializerModules>();
            builder.RegisterModule<CommerceApplicationAzureModule>();
            IContainer container = builder.Build();
            RootScope = container.BeginLifetimeScope();
        }
    }
}
