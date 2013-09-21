using System.Web.Mvc;

namespace Commerce.Web.Areas.Public
{
    public class PublicAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Public";
            }
        }

        // ** Note to Self: the usage of defaults to dictate route matches strikes a bit weird... no?
        public override void RegisterArea(AreaRegistrationContext context)
        {
            var namespaces = new[] { "Commerce.Web.Areas.Public.Controllers" };

            context.MapRoute(
                "Public Home",
                "page/{action}",
                new { controller = "Page", action = "Index" },
                namespaces);

            context.MapRoute(
                "Page Navigation",
                "",
                new { controller = "Page", action = "Index" },
                namespaces);

            context.MapRoute(
                "REST Routes with Id",
                "{controller}/{id}",
                new { action = "action-with-id"},
                namespaces);

            context.MapRoute(
                "REST Routes with no Id",
                "{controller}",
                new { action = "action" },
                namespaces);
        }
    }
}
