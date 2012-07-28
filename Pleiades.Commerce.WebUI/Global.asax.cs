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
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class CommerceHttpApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RegisterDIContainer();

            AreaRegistration.RegisterAllAreas();
            RegisterDefaultArea();

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

        public static void RegisterDefaultArea()
        {
            // Other than a bare minimum of defaults, these need to rolled into their own Area
            var routes = RouteTable.Routes;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Products - Root/List All",
                "",
                new { controller = "Products", action = "List", category = (string)null, page = UrlParameter.Optional });

            routes.MapRoute(
                "Products - All Categories by Page",
                "Page{page}",
                new { controller = "Products", action = "List", category = (string)null, page = "1" },
                new { page = @"\d+" });

            routes.MapRoute(
                "Products - List by Category",
                "{category}",
                new { controller = "Products", action = "List", page = "1" });

            routes.MapRoute(
                "Products - List by Category and Page",
                "{category}/Page{page}",
                new { controller = "Products", action = "List" });

            routes.MapRoute(
                "Product - Get Image",
                "Products/GetImage/{productid}",
                new { controller = "Products", action = "GetImage" });

            routes.MapRoute(
                "Products - 'failover route'",
                "{controller}/{action}",
                new { controller = "Products", action = "List" });
        }

        public static void RegisterGlobalFilters()
        {
            GlobalFilters.Filters.Add(new CustomErrorAttribute());
        }

    }
}
