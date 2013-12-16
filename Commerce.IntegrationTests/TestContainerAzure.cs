using Autofac;
using Commerce.Application;
using Commerce.Application.Azure;

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
            builder.RegisterModule<CommerceApplicationModule>();
            builder.RegisterModule<CommerceApplicationAzureModule>();
            var container = builder.Build();
            RootScope = container.BeginLifetimeScope();
        }
    }
}
