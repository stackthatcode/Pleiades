using System;
using System.Web;
using System.Web.Routing;

namespace Commerce.WebUI.Plumbing
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
            return new RouteValueDictionary(new { area = "Admin", controller = "Auth", action = "Login", });
        }

        public static RouteValueDictionary AdminLogout()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Auth", action = "Logout", });
        }

        public static RouteValueDictionary AdminManagerList()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Manager", action = "List", });
        }

        public static RouteValueDictionary AdminManagerDetails(object id)
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Manager", action = "Details", id = id });
        }
    }
}