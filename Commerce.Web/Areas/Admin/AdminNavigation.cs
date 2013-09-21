using System.Web.Routing;

namespace Commerce.Web.Areas.Admin
{
    // TODO: add ControllerObject.ToRoute() extension method
    // TODO: add a bit of glue which uses Controller Type with Actions verified by Expression Tree.  Voila!

    public class AdminNavigation
    {
        public static string ServerErrorView()
        {
            return "~/Areas/Admin/Views/Unsecured/ServerError.cshtml";
        }

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

        public static RouteValueDictionary ErrorTest()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Manager", action = "Error", });
        }

        public static RouteValueDictionary ManagerList()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Manager", action = "List", });
        }

        public static RouteValueDictionary ConfirmDeleteUser(int id)
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Manager", action = "DeleteConfirm", id = id });
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

        public static RouteValueDictionary ProductEditor()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Product", action = "Editor", });
        }

        public static RouteValueDictionary CreateNewOrder()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Order", action = "Create", });
        }

        public static RouteValueDictionary ManageOrders()
        {
            return new RouteValueDictionary(new { area = "Admin", controller = "Order", action = "Manage", });
        }
    }
}