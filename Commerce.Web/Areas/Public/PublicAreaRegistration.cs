using System.Web.Mvc;

namespace Commerce.Web.Areas.Admin
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
        }
    }
}
