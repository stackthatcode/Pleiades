using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Helpers
{
    public static class EnumHelpers
    {
        public static T StringToEnum<T>(this string value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return (T)Enum.Parse(typeof(T), value);
        }

        public static IEnumerable<int> ToEnumerableInt<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return Enum.GetValues(typeof(T)).Cast<int>();
        }

        public static bool Contains<T>(this List<T> actual, params T[] expected) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            return (expected.Any(x => actual.Contains(x)));
        }
    }
}
