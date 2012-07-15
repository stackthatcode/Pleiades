using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Framework.Identity.Model
{
    public enum AccountStatus
    {
        NotApplicable = 0,
        Disabled = 1,
        Active = 2,
        PaymentRequired = 3,
    }
}
