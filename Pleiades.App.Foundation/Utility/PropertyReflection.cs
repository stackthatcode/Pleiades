namespace Pleiades.App.Utility
{
    public static class PropertyReflection
    {
        public static bool StrungOutCompare<T>(this T input1, T input2) 
                where T : class
        {
            var type = typeof(T);

            foreach (var property in type.GetProperties())
            {
                if (!property.PropertyType.IsValueType && property.PropertyType.FullName != "System.String") continue;

                var value1 = property.GetValue(input1, null);
                var value2 = property.GetValue(input2, null);
                
                if (value1.ToString() != value2.ToString())
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
