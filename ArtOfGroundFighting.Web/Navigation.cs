using System.Web.Routing;
using ArtOfGroundFighting.Web.Controllers;
using Pleiades.Web.MvcHelpers;

namespace ArtOfGroundFighting.Web
{
    public class Navigation
    {
        public static string ServerErrorView()
        {
            return "~/Views/Page/ServerError.cshtml";
        }

        public static string Http400View()
        {
            return "~/Views/Page/NotFound.cshtml";
        }

        public static RouteValueDictionary Home()
        {
            return RouteValueDictionaryExtensions.FromController<PageController>(x => x.Index());
        }

        public static RouteValueDictionary TestHttp500()
        {
            return RouteValueDictionaryExtensions.FromController<PageController>(x => x.TestHttp500());
        }
    }
}