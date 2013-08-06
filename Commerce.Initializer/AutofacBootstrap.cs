using System;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using Pleiades.Injection;
using Pleiades.Web.Autofac;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Providers;
using Commerce.Web;

namespace Commerce.Initializer
{
    public class AutofacBootstrap
    {
        /// <summary>
        /// TODO: fix this when the Jihad against the static Membership class is complete
        /// </summary>
        public static IContainerAdapter CreateContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommerceInitializerModules>();
            var container = builder.Build();
            var containerAdapter = new AutofacContainer(container.BeginLifetimeScope());

            PfMembershipRepositoryBroker.RegisterFactory(() =>
            {
                var repository = containerAdapter.Resolve<IMembershipProviderRepository>();
                return repository;
            });
            return containerAdapter;
        }
    }
}
