using System;

namespace Pleiades.Framework.Helpers
{
    public static class StringExtensions
    {
        public static string TruncateAfter(this string input, int maxLength)
        {
            return input.Length > maxLength ? input.Substring(0, maxLength) + "..." : input;
        }
    }
}