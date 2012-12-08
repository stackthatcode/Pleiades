using System;
using System.Web;
using System.Web.Mvc;
using Pleiades.Injection;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Providers;
using Autofac;
using Autofac.Integration.Mvc;

namespace Commerce.WebUI.Plumbing
{
    public class AutofacBootstrap
    {
        public static IContainerAdapter RegisterAndWireIocContainer()
        {
            // Build the container
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommerceWebUIModule>();
            var container = builder.Build();

            // Wire container into ASP.NET MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Register the factory which creates the IMembershipProviderRepository dependency
            PfMembershipRepositoryBroker.RegisterFactory(() => container.Resolve<IMembershipProviderRepository>());

            return new AutofacContainer(container);
        }
    }
}