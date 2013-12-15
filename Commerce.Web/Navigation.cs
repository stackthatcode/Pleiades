using System.Web.Routing;
using Commerce.Web.Controllers;
using Pleiades.Web.MvcHelpers;

namespace Commerce.Web
{
    //
    // TODO: add ControllerObject.ToRoute() extension method
    //
    public class Navigation
    {
        public static string ServerErrorView()
        {
            return "~/Views/Unsecured/ServerError.cshtml";
        }

        public static RouteValueDictionary Home()
        {
            return RouteValueDictionaryExtensions.FromController<HomeController>(x => x.Index());
        }

        public static RouteValueDictionary Login()
        {
            return RouteValueDictionaryExtensions.FromController<UnsecuredController>(x => x.Login());
        }

        public static RouteValueDictionary Logout()
        {
            return RouteValueDictionaryExtensions.FromController<UnsecuredController>(x => x.Logout());
        }

        public static RouteValueDictionary ErrorTest()
        {
            return RouteValueDictionaryExtensions.FromController<ManagerController>(x => x.Error());
        }

        public static RouteValueDictionary ManagerList()
        {
            return RouteValueDictionaryExtensions.FromController<ManagerController>(x => x.List());
        }

        public static RouteValueDictionary ConfirmDeleteUser(int id)
        {
            var output = RouteValueDictionaryExtensions.FromController<ManagerController>(x => x.DeleteConfirm(12345));
            output["id"] = id;
            return output;
        }

        public static RouteValueDictionary ManagerDetails(object id)
        {
            var output = RouteValueDictionaryExtensions.FromController<ManagerController>(x => x.Details(12345));
            output["id"] = id;
            return output;
        }

        public static RouteValueDictionary CategoryEditor()
        {
            return RouteValueDictionaryExtensions.FromController<CategoryController>(x => x.Editor());
        }

        public static RouteValueDictionary SizeEditor()
        {
            return RouteValueDictionaryExtensions.FromController<SizeController>(x => x.Editor());
        }

        public static RouteValueDictionary SectionEditor()
        {
            return RouteValueDictionaryExtensions.FromController<SectionController>(x => x.Editor());
        }

        public static RouteValueDictionary BrandEditor()
        {
            return RouteValueDictionaryExtensions.FromController<BrandController>(x => x.Editor());
        }

        public static RouteValueDictionary ColorEditor()
        {
            return RouteValueDictionaryExtensions.FromController<ColorController>(x => x.Editor());
        }

        public static RouteValueDictionary ImageUploadTest()
        {
            return RouteValueDictionaryExtensions.FromController<ImageController>(x => x.UploadTest());
        }

        public static RouteValueDictionary ProductEditor()
        {
            return RouteValueDictionaryExtensions.FromController<ProductController>(x => x.Editor());
        }

        public static RouteValueDictionary CreateNewOrder()
        {
            return RouteValueDictionaryExtensions.FromController<CreateOrderController>(x => x.Create());
        }

        public static RouteValueDictionary ManageOrders()
        {
            return RouteValueDictionaryExtensions.FromController<ManageOrderController>(x => x.Editor());
        }
    }
}
