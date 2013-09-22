using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using Pleiades.Application.Utility;

namespace Pleiades.Web.MvcHelpers
{
    public class RouteValueDictionaryBuilder
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
            return FromController(actionFinder);
        }
    }
}
