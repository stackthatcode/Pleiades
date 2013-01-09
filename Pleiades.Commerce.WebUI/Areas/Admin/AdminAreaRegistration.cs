using System.Web.Mvc;

namespace Commerce.WebUI.Areas.Admin
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
            context.MapRoute(
                "Image Download",
                "Admin/Image/Download/{externalResourceId}",
                new { controller = "Image", action = "Download" });

            context.MapRoute(
                "Home Route",
                "Admin/",
                new { controller = "Home", action = "Index" });

            context.MapRoute(
                "Category Action w/ Parent Id, Child Id",
                "Admin/Category/{action}/{parentId}/{childId}",
                new { controller = "Category" }
            );

            context.MapRoute(
                "Category Action w/ Id",
                "Admin/Category/{action}/{id}",
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
                new { controller = "Category", action = "RetrieveAllSections" }
            );
        }
    }
}
