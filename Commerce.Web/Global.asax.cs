﻿using System;
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
            // Evil "new"?  Yes, but there's no other way since .NET runtime constructs the object
            GlobalFilters.Filters.Add(new SecurityAttribute());

            // TODO: respond to this one
            // GlobalFilters.Filters.Add(new CustomErrorAttribute());
        }

        public void MakeBundles()
        {
            // TODO: enable this via configuration
            BundleTable.EnableOptimizations = true;

            BundleTable.Bundles.Add(
                new ScriptBundle("~/Bundles/Foundation")
                    .Include("~/Content/JQuery/jquery-2.0.2.min.js")
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
                new StyleBundle("~/Bundles/KnockoutJS")
                    .Include("~/Content/Knockout/*.js"));

            BundleTable.Bundles.Add(
                new StyleBundle("~/Bundles/PushLibrary")
                    .Include("~/Content/PushLibrary/Scripts/*.js"));
        }
    }
}