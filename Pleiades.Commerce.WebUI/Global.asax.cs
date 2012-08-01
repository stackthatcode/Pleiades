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
        // TODO: move this to Navigation plumbing

        public static RouteValueDictionary HomeRoute()
        {
            return 
                new RouteValueDictionary(
                    new {
                            area = "Public",
                            controller = "Products",
                            action = "List",
                            category = (string)null, 
                        });
        }

        protected void Application_Start()
        {
            RegisterDIContainer();
            AreaRegistration.RegisterAllAreas();
            RegisterDefaultRoutes();
            RegisterGlobalFilters();

            //ModelBinders.Binders.Add(typeof(DomainUser), new DomainUserBinder());

            // Initialize Pleiades Security
            //var userservice = new DomainUserService();
            //userservice.Initialize();

            // Uncomment to enable Phil Haack's Tool
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

            // Direct traffic to ~/Public/Products/List
            RouteTable.Routes.MapRoute(
                "Default Route",
                String.Empty,   // URL
                new { area = "Public", controller = "Products", action = "List", });
        }

        public static void RegisterGlobalFilters()
        {
            GlobalFilters.Filters.Add(new CustomErrorAttribute());
        }
    }
}