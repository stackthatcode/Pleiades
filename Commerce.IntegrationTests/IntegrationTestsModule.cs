using System;
using Autofac;
using Commerce.Persist;
using Pleiades.Injection;
using Pleiades.Web;
using Pleiades.Web.Autofac;
using Pleiades.Web.Security;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Providers;

namespace Commerce.IntegrationTests
{
    public class IntegrationTestsModule : Module
    {
        static IContainerAdapter _containerAdapter = null;

        protected override void Load(ContainerBuilder builder)
        {
            // Autofac
            builder.RegisterType<AutofacContainer>().As<IContainerAdapter>().InstancePerLifetimeScope();

            // External Modules
            builder.RegisterModule<WebSecurityAggregateModule>();
            builder.RegisterModule<CommercePersistModule>();
        }

        public static IContainerAdapter Container()
        {
            if (_containerAdapter != null)
            {
                return _containerAdapter;
            }

            var builder = new ContainerBuilder();
            builder.RegisterModule<IntegrationTestsModule>();
            var container = builder.Build();

            _containerAdapter = new AutofacContainer(container.BeginLifetimeScope());

            PfMembershipRepositoryBroker.RegisterFactory(() =>
                {
                    return _containerAdapter.Resolve<IMembershipProviderRepository>();
                });
            return _containerAdapter;
        }
    }
}
