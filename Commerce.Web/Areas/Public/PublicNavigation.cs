using System.Web.Routing;
using Commerce.Web.Areas.Public.Controllers;
using Pleiades.Web.MvcHelpers;

namespace Commerce.Web.Areas.Public
{
    public class PublicNavigation
    {
        public static string AreaName = "Public";

        public static string ServerErrorView()
        {
            return "~/Areas/Public/Views/Page/ServerError.cshtml";
        }
        
        public static RouteValueDictionary Home()
        {
            return RouteValueDictionaryBuilder.FromController<PageController>(AreaName, x => x.Index());
        }

        public static RouteValueDictionary Contact()
        {
            return RouteValueDictionaryBuilder.FromController<PageController>(AreaName, x => x.Contact());
        }

        public static RouteValueDictionary TestHttp500()
        {
            return RouteValueDictionaryBuilder.FromController<PageController>(AreaName, x => x.TestHttp500());
        }
    }
}