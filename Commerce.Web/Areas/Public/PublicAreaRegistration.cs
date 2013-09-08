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

        public override void RegisterArea(AreaRegistrationContext context)
        {
            // Default Route - *KEEP*
            context.MapRoute(
                "Public Home Route",
                "",
                new { controller = "Products", action = "List" });

            context.MapRoute(
                "Public Controller + Action",
                "Public/{controller}/{action}",
                new { controller = "Home", action = "Index" });
        }
    }
}
