using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Pleiades.Commerce.WebUI.Plumbing.Model;
using Pleiades.Commerce.WebUI.Plumbing.Security;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Model;


namespace Pleiades.Commerce.WebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Products - Root/List All",
                "",
                new { controller = "Products", action = "List", category = (string)null, page = UrlParameter.Optional });

            routes.MapRoute(
                "Products - All Categories by Page",
                "Page{page}",
                new { controller = "Products", action = "List", category = (string)null, page = "1" },
                new { page = @"\d+"} );

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

        protected void Application_Start()
        {
            // Model Binders
            ModelBinders.Binders.Add(typeof(DomainUser), new DomainUserBinder());

            // Filters
            RegisterGlobalFilters(GlobalFilters.Filters);

            // Routes
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);

            // Initialize Pleiades Security
            var userservice = new DomainUserService();
            userservice.Initialize();

            // Uncomment to enable Phil Haack's Tool
            // RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }

    }
}
