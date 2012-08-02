using System;
using System.Web;
using System.Web.Routing;

namespace Pleiades.Commerce.WebUI.Plumbing.Navigation
{
    public class NavigationHelper
    {
        // TODO: move this to Navigation plumbing

        public static RouteValueDictionary HomeRoute()
        {
            return
                new RouteValueDictionary(
                    new
                    {
                        area = "Public",
                        controller = "Products",
                        action = "List",
                        category = (string)null,
                    });
        }
    }
}