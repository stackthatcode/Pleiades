using System;

namespace Pleiades.Application.Helpers
{
    public static class ParsingExtensions
    {
        public static int ToInt32(this object input)
        {
            return Int32.Parse(input.ToString());
        }
    }
}
