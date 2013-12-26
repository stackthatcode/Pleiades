using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Pleiades.App.Injection;

namespace ArtOfGroundFighting.Web.Plumbing
{
    public class Bootstrap
    {
        public static IContainerAdapter RegisterAndWireIocContainer()
        {
            // Build the container
            var builder = new ContainerBuilder();
            builder.RegisterModule<CompositionRoot>();
            var container = builder.Build();

            // Wire container into ASP.NET MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            return new AutofacContainer(container);
        }
    }
}
