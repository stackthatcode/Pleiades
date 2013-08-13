using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Commerce.Persist.Database;
using Pleiades.Web.Plumbing;
using Pleiades.Web.Security.Aspect;

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
            Database.SetInitializer<PushMarketContext>(null);

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

            // Bundle Optimization
            MakeBundles();

            // Phil Haack's Tool => ***SAVE***
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }

        public static void RegisterRoutes()
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        }

        public void RegisterGlobalFilters()
        {
            GlobalFilters.Filters.Add(new SecurityAttribute());
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
                new ScriptBundle("~/Bundles/BootstrapBaseScript")
                    .Include("~/Content/Bootstrap/js/bootstrap.min.js"));

            BundleTable.Bundles.Add(
                new StyleBundle("~/Bundles/BootstrapBaseStyle")
                    .Include("~/Content/Bootstrap/css/bootstrap.min.css"));

            BundleTable.Bundles.Add(
                new ScriptBundle("~/Bundles/BootstrapEnhancements")
                    .Include("~/Content/Bootstrap/js/bootstrap-alert.js")
                    .Include("~/Content/Bootstrap/js/bootstrap-dropdown.js")
                    .Include("~/Content/Bootstrap/js/bootstrap-modal.js")
                    .Include("~/Content/Bootstrap/js/bootstrap-popover.js")
                    .Include("~/Content/Bootstrap/js/bootstrap-tooltip.js"));

            BundleTable.Bundles.Add(
                new ScriptBundle("~/Bundles/KnockoutJS")
                    .Include("~/Content/Knockout/*.js"));

            BundleTable.Bundles.Add(
                new ScriptBundle("~/Bundles/PushLibrary")
                    .Include("~/Content/PushLibrary/*.js"));

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