using Autofac;
using Commerce.Initializer;

namespace Commerce.IntegrationTests
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
            builder.RegisterModule<CommerceInitializerModules>();
            IContainer container = builder.Build();
            RootScope = container.BeginLifetimeScope();
        }
    }
}
