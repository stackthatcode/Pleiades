using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Pleiades.Framework.Injection;
using Pleiades.Framework.MembershipProvider.Providers;
using Pleiades.Framework.Web.Security;
using Pleiades.Commerce.Initializer;
using Pleiades.Framework.Web.Security.Aspect;
using Pleiades.Commerce.WebUI.Plumbing.Autofac;
using Pleiades.Commerce.WebUI.Plumbing.ErrorHandling;

namespace Pleiades.Commerce.WebUI
{
    public class CommerceHttpApplication : HttpApplication
    {
        IGenericContainer Container { get; set; }

        protected void Application_Start()
        {
            // Components
            RegisterDIContainer();

            // Membership
            PfMembershipShimInit.SetFactory();

            // Routes
            AreaRegistration.RegisterAllAreas();
            RegisterDefaultRoutes();

            // Filters
            RegisterGlobalFilters();            

            // Model Binders
            // ModelBinders.Binders.Add(typeof(DomainUser), new DomainUserBinder());

            // Phil Haack's Tool => ***SAVE***
            // RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }

        public void RegisterDIContainer()
        {
            // Build the container
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommerceRootModule>();
            builder.RegisterControllers(typeof(CommerceHttpApplication).Assembly);
            var container = builder.Build();

            // Wire container into ASP.NET MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            this.Container = new AutofacContainer(container.BeginLifetimeScope());
        }

        public static void RegisterDefaultRoutes()
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        }

        public void RegisterGlobalFilters()
        {
            GlobalFilters.Filters.Add(this.Container.Resolve<PleiadesAuthorizeAttribute>());
            
            // TODO: respond to this one
            // GlobalFilters.Filters.Add(new CustomErrorAttribute());
        }
    }
}