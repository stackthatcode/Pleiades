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

        public static RouteValueDictionary Contact()
        {
            return new RouteValueDictionary(new { area = "Public", controller = "System", action = "Contact", });
        }
    }
}