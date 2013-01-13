using System;
using System.Web;
using System.Web.Routing;

namespace Commerce.WebUI.Plumbing
{
    // TODO: add ControllerObject.ToRoute() extension method

    public class AdminNavigation
    {
        public static RouteValueDictionary Home()
        {
            return new RouteValueDictionary( new { area = "Admin", controller = "Home", action = "Index", });
        }

        public static RouteValueDictionary Login()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Auth", action = "Login", });
        }

        public static RouteValueDictionary Logout()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Auth", action = "Logout", });
        }

        public static RouteValueDictionary ManagerList()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Manager", action = "List", });
        }

        public static RouteValueDictionary ManagerDetails(object id)
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Manager", action = "Details", id = id });
        }

        public static RouteValueDictionary CategoryEditor()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Category", action = "Editor", });
        }

        public static RouteValueDictionary SizeEditor()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Size", action = "Editor", });
        }

        public static RouteValueDictionary SectionEditor()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Section", action = "Editor", });
        }

        public static RouteValueDictionary BrandEditor()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Brand", action = "Editor", });
        }

        public static RouteValueDictionary ColorEditor()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Color", action = "Editor", });
        }

        public static RouteValueDictionary ImageUploadTest()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Image", action = "UploadTest", });
        }

    }
}