using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Pleiades.Application.Utility;

namespace Pleiades.Web.MvcHelpers
{
    public class MvcReflection
    {
        public static string ActionName<T>(Expression<Func<T, object>> actionFinder) where T : Controller
        {
            return ExpressionHelpers.UnaryMethodName(actionFinder);
        }

        public static string ControllerName<T>() where T : Controller
        {
            return typeof(T).Name.Replace("Controller", "");
        }
    }
}
