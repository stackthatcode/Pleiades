using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Framework.Helpers;

namespace Pleiades.Framework.Web.Security.Model
{
    public enum AuthorizationZone
    {
        Public = 1,
        Restricted = 2,
        Administrative = 3,
    }
}
