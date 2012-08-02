using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Pleiades.Commerce.WebUI.Plumbing.ErrorHandling;

namespace Pleiades.Commerce.WebUI
{
    public class CommerceHttpApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RegisterDIContainer();
            AreaRegistration.RegisterAllAreas();
            RegisterDefaultRoutes();
            RegisterGlobalFilters();

            // ModelBinders.Binders.Add(typeof(DomainUser), new DomainUserBinder());

            // Uncomment to enable Phil Haack's Tool => ***SAVE***
            // RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }

        public static void RegisterDIContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(CommerceHttpApplication).Assembly);
            builder.RegisterModule<CommerceModule>();
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        public static void RegisterDefaultRoutes()
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        }

        public static void RegisterGlobalFilters()
        {
            GlobalFilters.Filters.Add(new CustomErrorAttribute());
        }
    }
}