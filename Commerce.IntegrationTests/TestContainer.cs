using Autofac;
using Commerce.Initializer;

namespace Commerce.IntegrationTests
{
    public class TestContainer
    {
        static IContainer _container = null;
        static ILifetimeScope _rootScope = null;

        public static ILifetimeScope LifetimeScope()
        {
            return _rootScope.BeginLifetimeScope();
        }

        static TestContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommerceInitializerModules>();
            _container = builder.Build();
            _rootScope = _container.BeginLifetimeScope();
        }
    }
}
