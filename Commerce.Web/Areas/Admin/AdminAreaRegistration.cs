using System.Web.Mvc;

namespace Commerce.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        // TODO: fix my RESTful URI's, fuck-it-all with these wierd Routes

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var namespaces = new[] {"Commerce.Web.Areas.Admin.Controllers"};

            // Default Route - *KEEP*
            context.MapRoute(
                "Home Route",
                "Admin/",
                new { controller = "Home", action = "Index" },
                namespaces);

            context.MapRoute(
                "Inventory Total",
                "Admin/Product/{id}/InventoryTotal",
                new { controller = "Product", action = "InventoryTotal" },
                namespaces);

            // NOTE: this should be done entirely different
            context.MapRoute(
                "Inventory",
                "Admin/Product/{id}/Inventory",
                new { controller = "Product", action = "Inventory" },
                namespaces);

            // NOTE: this is superfluous
            context.MapRoute(
                "Image Download",
                "Admin/Image/Download/{externalResourceId}",
                new { controller = "Image", action = "Download" },
                namespaces);

            // This needs to be re-vamped - actually, we don't need it
            context.MapRoute(
                "Category Action + Parent Id + Child Id",
                "Admin/Category/{action}/{parentId}/{childId}",
                new { controller = "Category" },
                namespaces);

            context.MapRoute(
                "Admin Controller + Action + Id",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index" },
                namespaces);

            context.MapRoute(
                "Admin Controller + Action",
                "Admin/{controller}/{action}",
                new { controller = "Home", action = "Index" },
                namespaces);
        }
    }
}
