using System;
using System.Web;
using System.Web.Routing;

namespace Pleiades.Commerce.WebUI.Plumbing.Navigation
{
    public class OutboundNavigation
    {
        public static readonly RouteValueDictionary PublicHome = 
                new RouteValueDictionary(new { area = "Public", controller = "Products", action = "List", category = (string)null, });



        public static readonly RouteValueDictionary AdminHome =
                new RouteValueDictionary(new { area = "Admin", controller = "Home", action = "Index", });

        public static readonly RouteValueDictionary AdminLogin =
                new RouteValueDictionary(new { area = "Admin", controller = "Login", action = "Logon", });

    }
}