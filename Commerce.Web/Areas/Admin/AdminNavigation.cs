using System.Web.Routing;
using Commerce.Web.Areas.Admin.Controllers;
using Pleiades.Web.MvcHelpers;

namespace Commerce.Web.Areas.Admin
{
    // TODO: add ControllerObject.ToRoute() extension method

    public class AdminNavigation
    {
        public static string AreaName = "Admin";

        public static string ServerErrorView()
        {
            return "~/Areas/Admin/Views/Unsecured/ServerError.cshtml";
        }

        public static RouteValueDictionary Home()
        {
            return RouteValueDictionaryBuilder.FromController<HomeController>(AreaName, x => x.Index());
        }

        public static RouteValueDictionary Login()
        {
            return RouteValueDictionaryBuilder.FromController<UnsecuredController>(AreaName, x => x.Login());
        }

        public static RouteValueDictionary Logout()
        {
            return RouteValueDictionaryBuilder.FromController<UnsecuredController>(AreaName, x => x.Logout());
        }

        public static RouteValueDictionary ErrorTest()
        {
            return RouteValueDictionaryBuilder.FromController<ManagerController>(AreaName, x => x.Error());
        }

        public static RouteValueDictionary ManagerList()
        {
            return RouteValueDictionaryBuilder.FromController<ManagerController>(AreaName, x => x.List());
        }

        public static RouteValueDictionary ConfirmDeleteUser(int id)
        {
            var output = RouteValueDictionaryBuilder.FromController<ManagerController>(AreaName, x => x.DeleteConfirm(12345));
            output["id"] = id;
            return output;
        }

        public static RouteValueDictionary ManagerDetails(object id)
        {
            var output = RouteValueDictionaryBuilder.FromController<ManagerController>(AreaName, x => x.Details(12345));
            output["id"] = id;
            return output;
        }

        public static RouteValueDictionary CategoryEditor()
        {
            return RouteValueDictionaryBuilder.FromController<CategoryController>(AreaName, x => x.Editor());
        }

        public static RouteValueDictionary SizeEditor()
        {
            return RouteValueDictionaryBuilder.FromController<SizeController>(AreaName, x => x.Editor());
        }

        public static RouteValueDictionary SectionEditor()
        {
            return RouteValueDictionaryBuilder.FromController<SectionController>(AreaName, x => x.Editor());
        }

        public static RouteValueDictionary BrandEditor()
        {
            return RouteValueDictionaryBuilder.FromController<BrandController>(AreaName, x => x.Editor());
        }

        public static RouteValueDictionary ColorEditor()
        {
            return RouteValueDictionaryBuilder.FromController<ColorController>(AreaName, x => x.Editor());
        }

        public static RouteValueDictionary ImageUploadTest()
        {
            return RouteValueDictionaryBuilder.FromController<ImageController>(AreaName, x => x.UploadTest());
        }

        public static RouteValueDictionary ProductEditor()
        {
            return RouteValueDictionaryBuilder.FromController<ProductController>(AreaName, x => x.Editor());
        }

        public static RouteValueDictionary CreateNewOrder()
        {
            return RouteValueDictionaryBuilder.FromController<OrderController>(AreaName, x => x.Create());
        }

        public static RouteValueDictionary ManageOrders()
        {
            return RouteValueDictionaryBuilder.FromController<OrderController>(AreaName, x => x. Manage());
        }
    }
}
