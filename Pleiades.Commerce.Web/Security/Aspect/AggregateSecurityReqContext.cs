using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Framework.Identity.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class AggregateSecurityReqContext
    {
        public AuthorizationZone AuthorizationZone { get; set; }
        public AccountLevel AccountLevelRestriction { get; set; }
        public bool PaymentArea { get; set; }

        public SecurityRequirementsContext()
        {
            this.AuthorizationZone = AuthorizationZone.Public;
            this.AccountLevelRestriction = AccountLevel.Standard;
            this.PaymentArea = false;
        }
    }
}