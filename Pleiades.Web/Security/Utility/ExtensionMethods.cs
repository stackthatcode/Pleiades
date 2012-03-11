using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Web.Security.Utility
{
    public static class SecurityExtensionMethods
    {
        public static bool IsLeadAdmin(this string input)
        {            
            return ((input ?? "").Trim().ToUpper() == "ADMIN");
        }
    }
}
