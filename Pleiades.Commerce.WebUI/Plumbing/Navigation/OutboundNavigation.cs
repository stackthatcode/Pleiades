using System;
using System.Web;
using System.Web.Routing;

namespace Pleiades.Commerce.WebUI.Plumbing.Navigation
{
    // TODO: add ControllerObject.ToRoute() extension method

    public class OutboundNavigation
    {
        public static RouteValueDictionary PublicHome()
        {
            return new RouteValueDictionary(
                new { area = "Public", controller = "Products", action = "List", category = (string)null, });
        }

        public static RouteValueDictionary AdminHome()
        {
            return new RouteValueDictionary(
                new { area = "Admin", controller = "Home", action = "Index", });
        }

        public static RouteValueDictionary AdminLogin()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Login", action = "Authenticate", });
        }
    }
}