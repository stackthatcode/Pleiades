using System.Web.Routing;
using Commerce.Web.Controllers;
using Pleiades.Web.MvcHelpers;

namespace Commerce.Web
{
    // TODO: add ControllerObject.ToRoute() extension method

    public class AdminNavigation
    {
        public static string ServerErrorView()
        {
            return "~/Views/Unsecured/ServerError.cshtml";
        }

        public static RouteValueDictionary Home()
        {
            return RouteValueDictionaryBuilder.FromController<HomeController>(x => x.Index());
        }

        public static RouteValueDictionary Login()
        {
            return RouteValueDictionaryBuilder.FromController<UnsecuredController>(x => x.Login());
        }

        public static RouteValueDictionary Logout()
        {
            return RouteValueDictionaryBuilder.FromController<UnsecuredController>(x => x.Logout());
        }

        public static RouteValueDictionary ErrorTest()
        {
            return RouteValueDictionaryBuilder.FromController<ManagerController>(x => x.Error());
        }

        public static RouteValueDictionary ManagerList()
        {
            return RouteValueDictionaryBuilder.FromController<ManagerController>(x => x.List());
        }

        public static RouteValueDictionary ConfirmDeleteUser(int id)
        {
            var output = RouteValueDictionaryBuilder.FromController<ManagerController>(x => x.DeleteConfirm(12345));
            output["id"] = id;
            return output;
        }

        public static RouteValueDictionary ManagerDetails(object id)
        {
            var output = RouteValueDictionaryBuilder.FromController<ManagerController>(x => x.Details(12345));
            output["id"] = id;
            return output;
        }

        public static RouteValueDictionary CategoryEditor()
        {
            return RouteValueDictionaryBuilder.FromController<CategoryController>(x => x.Editor());
        }

        public static RouteValueDictionary SizeEditor()
        {
            return RouteValueDictionaryBuilder.FromController<SizeController>(x => x.Editor());
        }

        public static RouteValueDictionary SectionEditor()
        {
            return RouteValueDictionaryBuilder.FromController<SectionController>(x => x.Editor());
        }

        public static RouteValueDictionary BrandEditor()
        {
            return RouteValueDictionaryBuilder.FromController<BrandController>(x => x.Editor());
        }

        public static RouteValueDictionary ColorEditor()
        {
            return RouteValueDictionaryBuilder.FromController<ColorController>(x => x.Editor());
        }

        public static RouteValueDictionary ImageUploadTest()
        {
            return RouteValueDictionaryBuilder.FromController<ImageController>(x => x.UploadTest());
        }

        public static RouteValueDictionary ProductEditor()
        {
            return RouteValueDictionaryBuilder.FromController<ProductController>(x => x.Editor());
        }

        public static RouteValueDictionary CreateNewOrder()
        {
            return RouteValueDictionaryBuilder.FromController<CreateOrderController>(x => x.Create());
        }

        public static RouteValueDictionary ManageOrders()
        {
            return RouteValueDictionaryBuilder.FromController<ManageOrderController>(x => x.Editor());
        }
    }
}
