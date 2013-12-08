using System;
using System.Web;
using System.Web.Routing;

namespace Commerce.WebUI.Plumbing
{
    public class PublicNavigation
    {
        public static RouteValueDictionary Home()
        {
            return new RouteValueDictionary(
                new { area = "Public", controller = "Products", action = "List", category = (string)null, });
        }
    }
}