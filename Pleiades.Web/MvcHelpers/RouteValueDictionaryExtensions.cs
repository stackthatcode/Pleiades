using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using Pleiades.App.Utility;

namespace Pleiades.Web.MvcHelpers
{
    public static class RouteValueDictionaryExtensions
    {
        public static RouteValueDictionary FromController<T>(string areaName, Expression<Func<T, object>> actionFinder)
                where T : Controller
        {
            var actionName = ExpressionHelpers.UnaryMethodName(actionFinder);
            var controllerName = typeof(T).Name.Replace("Controller", "");
            return new RouteValueDictionary(new { area = areaName, controller = controllerName, action = actionName, });
        }

        public static RouteValueDictionary FromController<T>(Expression<Func<T, object>> actionFinder)
                where T : Controller
        {
            var actionName = ExpressionHelpers.UnaryMethodName(actionFinder);
            var controllerName = typeof(T).Name.Replace("Controller", "");
            return new RouteValueDictionary(new { controller = controllerName, action = actionName, });
        }

        public static string ToUrl(this RouteValueDictionary input, UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl(input);
        }
    }
}
