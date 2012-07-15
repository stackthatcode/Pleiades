using System;

namespace Pleiades.Framework.Utility
{
    public static class EnumExtensions
    {
        public static T ParseToEnum<T>(this string input) where T : struct, IConvertible
        {
            return (T)Enum.Parse(typeof(T), input);
        }
    }
}
