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
                "Products - 'Public no Id route'",
                String.Empty,
                new { controller = "Products", action = "List" });

            context.MapRoute(
                "Products - 'default route'",
                "Public/{controller}/{action}",
                new { controller = "Products", action = "List" });

        }
    }
}
