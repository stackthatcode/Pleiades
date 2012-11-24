using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Pleiades.Injection;
using Pleiades.Web.Security.Providers;
using Pleiades.Web.Security;
using CommerceInitializer;
using Pleiades.Web.Security.Aspect;
using Commerce.WebUI.Plumbing.Autofac;
using Commerce.WebUI.Plumbing.ErrorHandling;

namespace Commerce.WebUI
{
    public class CommerceHttpApplication : HttpApplication
    {
        IServiceLocator Container { get; set; }

        protected void Application_Start()
        {
            // Routes
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes();

            // Components
            RegisterDIContainer();

            // Membership
            MembershipRepositoryShim.SetFactory();

            // Filters
            RegisterGlobalFilters();            

            // Model Binders
            // ModelBinders.Binders.Add(typeof(DomainUser), new DomainUserBinder());

            // Phil Haack's Tool => ***SAVE***
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }

        public void RegisterDIContainer()
        {
            // Build the container
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommerceWebUIModule>();
            builder.RegisterControllers(typeof(CommerceHttpApplication).Assembly);
            var container = builder.Build();

            // Wire container into ASP.NET MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            this.Container = new AutofacContainer(container.BeginLifetimeScope());
        }

        public static void RegisterRoutes()
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        }

        public void RegisterGlobalFilters()
        {
            GlobalFilters.Filters.Add(this.Container.Resolve<SecurityAttribute>());
            
            // TODO: respond to this one
            // GlobalFilters.Filters.Add(new CustomErrorAttribute());
        }
    }
}