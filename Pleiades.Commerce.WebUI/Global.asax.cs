using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Pleiades.Injection;
using Pleiades.Web.Security.Providers;
using Pleiades.Web.Security;
using Pleiades.Web.Security.Aspect;
using Commerce.WebUI.Plumbing;

namespace Commerce.WebUI
{
    public class CommerceHttpApplication : HttpApplication
    {
        protected void Application_Start()
        {
            // Routes
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes();

            // Components
            AutofacBootstrap.RegisterAndWireIocContainer();

            // Filters
            RegisterGlobalFilters();

            // Phil Haack's Tool => ***SAVE***
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }

        public static void RegisterRoutes()
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        }

        public void RegisterGlobalFilters()
        {
            // Evil "new"?  Yes, but there's no other way since .NET runtime constructs the object
            GlobalFilters.Filters.Add(new SecurityAttribute());
            
            // TODO: respond to this one
            // GlobalFilters.Filters.Add(new CustomErrorAttribute());
        }
    }
}