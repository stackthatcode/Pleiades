using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Utilities.General;

namespace Pleiades.Web.Security.Model
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
