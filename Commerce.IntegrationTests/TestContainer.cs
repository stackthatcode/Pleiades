using Autofac;
using Commerce.Application;

namespace ArtOfGroundFighting.IntegrationTests
{
    public class TestContainer
    {
        static readonly ILifetimeScope RootScope = null;

        public static ILifetimeScope LifetimeScope()
        {
            return RootScope.BeginLifetimeScope();
        }

        static TestContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommerceApplicationModule>();
            var container = builder.Build();
            RootScope = container.BeginLifetimeScope();
        }
    }
}
