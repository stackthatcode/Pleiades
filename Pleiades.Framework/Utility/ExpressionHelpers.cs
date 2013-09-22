using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Pleiades.Application.Utility
{
    public class ExpressionHelpers
    {
        public static string MemberName<T, TProp>(Expression<Func<T, TProp>> expression)
        {
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
                return null;

            return memberExpression.Member.Name;
        }

        public static string UnaryMethodName<T, TProp>(Expression<Func<T, TProp>> expression)
        {
            var convert = expression.Body as MethodCallExpression;
            return convert.Method.Name;
        }
    }
}
