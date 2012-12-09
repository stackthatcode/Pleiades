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
        protected override void Load(ContainerBuilder builder)
        {
            // Autofac
            builder.RegisterType<AutofacContainer>().As<IContainerAdapter>().InstancePerLifetimeScope();

            // External Modules
            builder.RegisterModule<WebSecurityAggregateModule>();
            builder.RegisterModule<CommercePersistModule>();
        }

        public static IContainerAdapter CreateContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<IntegrationTestsModule>();
            var container = builder.Build();

            var containerAdapter = new AutofacContainer(container.BeginLifetimeScope());

            PfMembershipRepositoryBroker.RegisterFactory(() => 
                {
                    return containerAdapter.Resolve<IMembershipProviderRepository>();
                });
            return containerAdapter;
        }
    }
}
