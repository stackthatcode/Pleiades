using System.Web.Mvc;
using System.Web.Routing;

namespace Commerce.Web.Plumbing
{
    public class RouteRegistration
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // Default Route - *KEEP*
            routes.MapRoute(
                "Home Route",
                "",
                new {controller = "Home", action = "Index"});

            routes.MapRoute(
                "Inventory Total",
                "Product/{id}/InventoryTotal",
                new { controller = "Product", action = "InventoryTotal" });

            // NOTE: this should be done entirely different
            routes.MapRoute(
                "Inventory",
                "Product/{id}/Inventory",
                new { controller = "Product", action = "Inventory" });

            // NOTE: this is superfluous
            routes.MapRoute(
                "Image Download",
                "Image/Download/{externalResourceId}",
                new { controller = "Image", action = "Download" });

            // This needs to be re-vamped - actually, we don't need it
            routes.MapRoute(
                "Category Action + Parent Id + Child Id",
                "Category/{action}/{parentId}/{childId}",
                new { controller = "Category" });

            routes.MapRoute(
                "Controller + Action + Id",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index" });

            routes.MapRoute(
                "Admin Controller + Action",
                "{controller}/{action}",
                new { controller = "Home", action = "Index" });
        }
    }
}
