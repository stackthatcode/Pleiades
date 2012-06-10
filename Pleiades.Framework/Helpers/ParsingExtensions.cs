using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Framework.Helpers
{
    public static class ParsingExtensions
    {
        public static int ToInt32(this object input)
        {
            return Int32.Parse(input.ToString());
        }
    }
}
