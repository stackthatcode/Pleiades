using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Framework.Helpers;

namespace Pleiades.Framework.Identity.Model
{
    /// <summary>
    /// Account Level enumerates the different levels of Users - based on their subscription type
    /// </summary>
    public enum AccountLevel
    {
        NotApplicable = 0,
        Standard = 1,
        Gold = 2,
    };
}
