using System;
using System.Web;
using System.Web.Mvc;
using Pleiades.Injection;
using Pleiades.Web.Autofac;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Providers;
using Autofac;
using Autofac.Integration.Mvc;

namespace Commerce.WebUI
{
    public class AutofacBootstrap
    {
        public static IContainerAdapter RegisterAndWireIocContainer()
        {
            // Build the container
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommerceWebUIModule>();
            var container = builder.Build();

            // Register the factory which creates the IMembershipProviderRepository dependency
            PfMembershipRepositoryBroker.RegisterFactory(() => 
                {
                    // This ensures that Membership gets dependencies tied to the current Request Lifetime Scope
                    var _container = DependencyResolver.Current.GetService<IContainerAdapter>();
                    return _container.Resolve<IMembershipProviderRepository>();
                });

            // Wire container into ASP.NET MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            return new AutofacContainer(container);
        }
    }
}