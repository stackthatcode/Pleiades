using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Utilities.General;

namespace Pleiades.Web.Security.Model
{
    public enum AuthorizationZone
    {
        Public = 1,
        Restricted = 2,
        Administrative = 3,
    }
}
