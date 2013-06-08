using System;
using System.Web;
using System.Web.Routing;

namespace Commerce.Web.Plumbing
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