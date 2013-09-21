using System.Web.Routing;

namespace Commerce.Web.Areas.Public
{
    public class PublicNavigation
    {
        public static string ServerErrorView()
        {
            return "~/Areas/Public/Views/Page/ServerError.cshtml";
        }
        
        public static RouteValueDictionary Home()
        {
            return new RouteValueDictionary(new { area="Public", controller = "Page", action = "Index" });
        }

        public static RouteValueDictionary Contact()
        {
            return new RouteValueDictionary(new { area = "Public", controller = "Page", action = "Contact", });
        }

        public static RouteValueDictionary TestHttp500()
        {
            return new RouteValueDictionary(new { area = "Public", controller = "Page", action = "TestHttp500", });
        }
    }
}