using System;
using System.Web.Mvc;

namespace Commerce.WebUI.Areas.Public
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

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Products - All Categories by Page",
                "Public/Page{page}",
                new { controller = "Products", action = "List", category = (string)null, page = "1" },
                new { page = @"\d+" });

            context.MapRoute(
                "Products - List by Category",
                "Public/{category}",
                new { controller = "Products", action = "List", page = "1" });

            context.MapRoute(
                "Products - List by Category and Page",
                "Public/{category}/Page{page}",
                new { controller = "Products", action = "List" });

            context.MapRoute(
                "Product - Get Image",
                "Public/Products/GetImage/{productid}",
                new { controller = "Products", action = "GetImage" });

            context.MapRoute(
                "Products - 'failover route'",
                "Public/{controller}/{action}",
                new { controller = "Products", action = "List" });

            context.MapRoute(
                "Products - 'default route'",
                String.Empty,
                new { controller = "Products", action = "List" });

        }
    }
}
