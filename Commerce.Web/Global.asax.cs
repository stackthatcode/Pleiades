using System;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Pleiades.Injection;
using Pleiades.Web.Plumbing;
using Pleiades.Web.Security.Providers;
using Pleiades.Web.Security;
using Pleiades.Web.Security.Aspect;
using Commerce.Persist.Concrete;
using Commerce.Web.Plumbing;

namespace Commerce.Web
{
    public class CommerceWebApplication : System.Web.HttpApplication
    {
        //protected void Application_Start()
        //{
        //    AreaRegistration.RegisterAllAreas();

        //    WebApiConfig.Register(GlobalConfiguration.Configuration);
        //    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        //    RouteConfig.RegisterRoutes(RouteTable.Routes);
        //}

        protected void Application_Start()
        {
            // Prevent Entity Framework foolishness
            Database.SetInitializer<PleiadesContext>(null);

            // Routes
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes();

            // Components
            AutofacBootstrap.RegisterAndWireIocContainer();

            // Bit of glue to make JSON POST-ing work
            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());

            // Model Binders
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());

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

        public void MakeBundles()
        {
            //BundleTable.EnableOptimizations = true;
            //BundleTable.Bundles.Add(
            //    new ScriptBundle("~/bundles/baseScripts")
            //        .Include()
            //        .Include()
            //        .Include()

            //    );
        }

    }
}