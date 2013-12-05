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
using Commerce.Web.Plumbing;

namespace Commerce.Web
{
    public class CommerceWebApplication : HttpApplication
    {
        protected void Application_Start()
        {
            // Prevent Entity Framework foolishness
            Database.SetInitializer<PushMarketContext>(null);

            // Routes
            RouteRegistration.RegisterRoutes(RouteTable.Routes);
            RegisterSystemRoutes();

            // Components
            AutofacBootstrap.RegisterAndWireIocContainer();

            // Bit of glue to make JSON POST-ing work
            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());

            // Model Binders
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());

            // Filters
            RegisterGlobalFilters();

            // Bundle Optimization
            MakeBundles();

            // Logger
            LoggerSingleton.Get = NLoggerImpl.RegistrationFactory("Commerce.Web", ActivityId.MessageFormatter);

            // Phil Haack's Tool
            if (ConfigurationManager.AppSettings["EnableRouteDebug"].ToBoolTryParse())
            {
                RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var lastError = Server.GetLastError();
            LoggerSingleton.Get().Error(lastError);
            ErrorNotification.Send(lastError);

            Server.ClearError();
            var redirectUrl = ConfigurationManager.AppSettings["AdminErrorRedirect"];
            if (redirectUrl != null) 
            {
                HttpContext.Current.Response.Redirect(redirectUrl);
            }
        }
        
        // TODO: how do we manage 404's in the Admin Site...?
        public static void RegisterSystemRoutes()
        {
            // TODO: need a 404 for the Admin
            
            //RouteTable.Routes.MapRoute(
            //    "404-PageNotFound",
            //    "{*url}",
            //    new {area="Public", controller = "System", action = "NotFound"},
            //    new [] {"Commerce.Web.Areas.Public.Controllers"}
            //    );

            //RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
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
                new ScriptBundle("~/Bundles/BootstrapBaseScript_2_3_2")
                    .Include("~/Content/Bootstrap_2_3_2/js/bootstrap.min.js"));
            
            BundleTable.Bundles.Add(
                new ScriptBundle("~/Bundles/BootstrapBaseScript_3_0_0")
                    .Include("~/Content/Bootstrap_3_0_0/js/bootstrap.min.js"));

            BundleTable.Bundles.Add(
                new ScriptBundle("~/Bundles/BootstrapEnhancements_2_3_2")
                    .Include("~/Content/Bootstrap_2_3_2/js/bootstrap-alert.js")
                    .Include("~/Content/Bootstrap_2_3_2/js/bootstrap-dropdown.js")
                    .Include("~/Content/Bootstrap_2_3_2/js/bootstrap-modal.js")
                    .Include("~/Content/Bootstrap_2_3_2/js/bootstrap-popover.js")
                    .Include("~/Content/Bootstrap_2_3_2/js/bootstrap-tooltip.js"));

            BundleTable.Bundles.Add(
                new ScriptBundle("~/Bundles/KnockoutJS")
                    .Include("~/Content/Knockout/*.js"));

            BundleTable.Bundles.Add(
                new ScriptBundle("~/Bundles/PushLibraryKo")
                    .Include("~/Content/PushLibraryKo/*.js"));

            BundleTable.Bundles.Add(
                new ScriptBundle("~/Bundles/FineUploaderJs")
                    .Include("~/Content/FineUploader/js/util.js")
                    .Include("~/Content/FineUploader/js/header.js")
                    .Include("~/Content/FineUploader/js/button.js")
                    .Include("~/Content/FineUploader/js/handler.base.js")
                    .Include("~/Content/FineUploader/js/handler.form.js")
                    .Include("~/Content/FineUploader/js/handler.xhr.js")
                    .Include("~/Content/FineUploader/js/uploader.basic.js")
                    .Include("~/Content/FineUploader/js/dnd.js")
                    .Include("~/Content/FineUploader/js/uploader.js")
                    .Include("~/Content/FineUploader/js/jquery-plugin.js"));
            
            BundleTable.Bundles.Add(
                new StyleBundle("~/Bundles/FineUploaderCss")
                    .Include("~/Content/FineUploader/fineuploader.css"));
        }
    }
}