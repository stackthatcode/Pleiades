using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Helpers;

namespace Pleiades.Security
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
