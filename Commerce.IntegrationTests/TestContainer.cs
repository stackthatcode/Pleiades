using System;
using Autofac;
using Commerce.Initializer;
using Commerce.Persist;
using Pleiades.Web;
using Pleiades.Web.Autofac;
using Pleiades.Web.Security;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Providers;

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

            // And yet, still more reasons to absolutely hate the Membership Provider in it's current, legacy state
            PfMembershipRepositoryBroker.RegisterFactory(() =>
            {
                // This will effectively be a singleton, since the Instance Lifetime is scoped by the root
                // NOTE: this is actually quite fucking evil, man!  Once we clean up the Membership Provider stuff, we're cool
                return _rootScope.Resolve<IMembershipProviderRepository>();
            });
        }
    }
}
