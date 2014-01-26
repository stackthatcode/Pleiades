using System;
using System.Configuration;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Pleiades.App.Logging;
using Pleiades.App.Utility;
using Pleiades.Web.Activity;
using Pleiades.Web.Plumbing;
using Pleiades.Web.Security.Aspect;
using Commerce.Application.Database;
using ArtOfGroundFighting.Web.Plumbing;

namespace ArtOfGroundFighting.Web
{
    public class ArtOfGroundFightingApplication : HttpApplication
    {
        protected void Application_Start()
        {
            // Prevent Entity Framework foolishness
            Database.SetInitializer<PushMarketContext>(null);

            // Routes
            RouteRegistration.RegisterRoutes(RouteTable.Routes);
            
            // Components
            Bootstrap.RegisterAndWireIocContainer();

            // Bit of glue to make JSON POST-ing work
            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());

            // Model Binders
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());

            // Filters
            RegisterGlobalFilters();

            // Bundle Optimization
            MakeBundles();

            // Logger
            LoggerRegistration.Register();

            // Phil Haack's Tool
            if (ConfigurationManager.AppSettings["EnableRouteDebug"].ToBoolTryParse())
            {
                RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (!ConfigurationManager.AppSettings["ErrorHandlingEnabled"].ToBoolTryParse())
            {
                return;
            }

            var lastError = Server.GetLastError();
            var httpException = new HttpException(null, lastError);

            LoggerSingleton.Get().Error(lastError);

            if (httpException.GetHttpCode() != 404)
            {
                ErrorNotification.Send(lastError);
            }

            Server.ClearError();
            var redirectUrl = ConfigurationManager.AppSettings["AdminErrorRedirect"];
            if (redirectUrl != null)
            {
                HttpContext.Current.Response.Redirect(redirectUrl);
            }
        }

        public void RegisterGlobalFilters()
        {
            // MVC "classic" filters
            GlobalFilters.Filters.Add(new SecurityAttribute());
            GlobalFilters.Filters.Add(new HandleErrorAttributeImpl());
        }

        public void MakeBundles()
        {
            BundleTable.EnableOptimizations = true;

            BundleTable.Bundles.Add(
                new ScriptBundle("~/Bundles/Foundation")
                    .Include("~/Content/JQuery/jquery-2.0.3.min.js")
                    .Include("~/Content/JQuery/jquery.validate.min.js")
                    .Include("~/Content/Utilities/*.js"));

            BundleTable.Bundles.Add(
                new ScriptBundle("~/Bundles/BootstrapBaseScript_3_0_0")
                    .Include("~/Content/Bootstrap_3_0_0/js/bootstrap.min.js"));
        }
    }
}
