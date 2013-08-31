using System;
using Autofac;
using Commerce.Initializer;
using Commerce.Application;
using Pleiades.Web;
using Pleiades.Web.Autofac;
using Pleiades.Web.Security;
using Pleiades.Web.Security.Interface;

namespace Commerce.IntegrationTests
{
    public class TestContainer
    {
        static IContainer _container = null;
        static ILifetimeScope _rootScope = null;

        public static ILifetimeScope LifetimeScope()
        {
            return _rootScope;
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
