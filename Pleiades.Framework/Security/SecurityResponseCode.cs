using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Framework.Helpers;

namespace Pleiades.Framework.Security
{
    /// <summary>
    /// Universal Security Response Codes
    /// </summary>
    public enum SecurityResponseCode
    {
        Allowed,
        AccessDenied,
        AccessDeniedSolicitLogon,
        AccountDisabledNonPayment,
        AccessDeniedToAccountLevel,
    };
}
