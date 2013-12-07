using System.Web.Mvc;
using System.Web.Routing;

namespace Commerce.ArtOfGroundFighting.Plumbing
{
    public class RouteRegistration
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "Public Home",
                "page/{action}",
                new { controller = "Page", action = "Index" });

            routes.MapRoute(
                "Page Navigation",
                "",
                new { controller = "Page", action = "Index" });

            routes.MapRoute(
                "System Pages",
                "system/{action}",
                new { controller = "System", action = "ServerError" });

            routes.MapRoute(
                "REST Routes with Id",
                "{controller}/{id}",
                new { action = "action-with-id" });

            routes.MapRoute(
                "REST Routes with no Id",
                "{controller}",
                new { action = "action" });
        }
    }
}
