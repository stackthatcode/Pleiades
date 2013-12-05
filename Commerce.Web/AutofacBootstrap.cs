using System.Web.Mvc;
using Commerce.Web.Injection;
using Autofac;
using Autofac.Integration.Mvc;
using Pleiades.App.Injection;

namespace Commerce.Web
{
    public class AutofacBootstrap
    {
        public static IContainerAdapter RegisterAndWireIocContainer()
        {
            // Build the container
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommerceWebModule>();
            var container = builder.Build();

            // Wire container into ASP.NET MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            return new AutofacContainer(container);
        }
    }
}