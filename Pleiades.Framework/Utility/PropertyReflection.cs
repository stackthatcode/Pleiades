using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Pleiades.Framework.Utilities
{
    public static class PropertyReflection
    {
        public static bool ValuePropertyCompare<T>(this T input1, T input2) 
                where T : class
        {
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                if (!property.PropertyType.IsValueType) continue;

                var value1 = property.GetValue(input1, new object[0]);
                var value2 = property.GetValue(input2, new object[0]);

                if (value1 != value2)
                {
                    return false;
                }
            }

            return true;
        }


        // DON'T DO THIS!!!  USE AUTOMAPPER
        //public static bool ValuePropertyCompareByProjection<T1, T2>(this T1 projectFrom, T2 projectTo) 
        //        where T1 : class 
        //        where T2 : class
        //{
        //    var typeFrom = typeof(T1);
        //    var typeTo = typeof(T2);
        //    var propertiesFrom = typeFrom.GetProperties();

        //    foreach (var propertyFrom in propertiesFrom)
        //    {
        //        if (!propertyFrom.PropertyType.IsValueType) continue;

        //        if (typeTo.GetProperties.
        //    }

        //    return true;
        //}
    }
}
