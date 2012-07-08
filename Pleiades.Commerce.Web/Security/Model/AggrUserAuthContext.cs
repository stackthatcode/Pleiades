using System;
using System.Web;
using Pleiades.Commerce.Domain.Entities.Users;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Security;

namespace Pleiades.Commerce.Web.Security.Model
{
    public class AggrUserAuthContext : IIdentityAuthorizationContext
    {
        public AggrUserAuthContext()
        {
            this.SecurityResponseCode = SecurityResponseCode.Allowed;
            this.ExecutionStateValid = true;
        }

        // Http 
        public HttpContextBase HttpContext { get; set; }

        // User
        public AggregateUser AggregateUser { get; set; }
        public IdentityUser CurrentUser { get { return AggregateUser == null ? null : AggregateUser.IdentityUser; } }

        // Identity Authorization Requirements
        public AuthorizationZone AuthorizationZone { get; set; }
        public AccountLevel AccountLevelRestriction { get; set; }
        public bool IsPaymentArea { get; set; }
        public int? ResourceOwnerId { get; set;  }

        // Step Execution Results
        public SecurityResponseCode SecurityResponseCode { get; set; }
        public bool ExecutionStateValid { get; set; }
    }
}
