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

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // Default Route - *KEEP*
            context.MapRoute(
                "Home Route",
                "Admin/",
                new { controller = "Home", action = "Index" });

            // TODO: fix my RESTful URI's, fuck-it-all with these wierd Routes
            context.MapRoute(
                "Inventory Total",
                "Admin/Product/{id}/InventoryTotal",
                new { controller = "Product", action = "InventoryTotal" });

            // NOTE: this should be done entirely different
            context.MapRoute(
                "Inventory",
                "Admin/Product/{id}/Inventory",
                new { controller = "Product", action = "Inventory" });

            // NOTE: this is superfluous
            context.MapRoute(
                "Image Download",
                "Admin/Image/Download/{externalResourceId}",
                new { controller = "Image", action = "Download" });

            // This needs to be re-vamped - actually, we don't need it
            context.MapRoute(
                "Category Action w/ Parent Id, Child Id",
                "Admin/Category/{action}/{parentId}/{childId}",
                new { controller = "Category" }
            );

            context.MapRoute(
                "Admin Controller, Action, Id",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index" }
            );

            context.MapRoute(
                "Admin Default",
                "Admin/{controller}/{action}",
                new { controller = "Home", action = "Index" }
            );
        }
    }
}
