using System.Web.Mvc;

namespace Pleiades.Commerce.WebUI.Areas.Admin
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
                "Home Route",
                "Admin/",
                new { controller = "Home", action = "Index" }
            );

            context.MapRoute(
                "Category Table With Pages and Sorting",
                "Admin/Category/{action}/Page{page}/Sort/{sortIndex}",
                new { controller = "Category", action = "Rows" }
            );

            context.MapRoute(
                "Category Page Sorting",
                "Admin/Category/PageById/{id}/Sort/{sortIndex}",
                new { controller = "Category", action = "PageById" }
            );

            context.MapRoute(
                "Category Table With Pages",
                "Admin/Category/{action}/Page{page}",
                new { controller = "Category", action = "Rows", sortIndex = 1 }
            );

            context.MapRoute(
                "Category Action w/ Id",
                "Admin/Category/{action}/{id}",
                new { controller = "Category" }
            );

            context.MapRoute(
                "Admin List With Pages",
                "Admin/AdminManager/List/Page{page}",
                new { controller = "AdminManager", action = "List" }
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
