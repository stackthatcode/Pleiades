using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Utilities.General
{
    public static class GenericExtensions
    {
        public static int ToInt32(this object input)
        {
            return Int32.Parse(input.ToString());
        }

        public static string TruncateAfter(this string input, int maxLength)
        {
            return input.Length > maxLength ? input.Substring(0, maxLength) + "..." : input;                    
        }
    }
}
