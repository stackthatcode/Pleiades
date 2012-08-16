using System;
using System.Web;
using Pleiades.Commerce.Domain.Model.Users;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Security;

namespace Pleiades.Framework.Web.Security.Model
{
    public class SystemAuthorizationContextBase : ISystemAuthorizationContext
    {
        public SystemAuthorizationContextBase()
        {
            this.SecurityResponseCode = SecurityResponseCode.Allowed;
            this.IsExecutionStateValid = true;
        }

        // Http 
        public HttpContextBase HttpContext { get; set; }

        // User
        public AggregateUser ThisUser { get; set; }
        public IdentityUser CurrentIdentity { get { return ThisUser == null ? null : ThisUser.IdentityUser; } }

        // Identity-based System-level Authorization Requirements
        public AuthorizationZone AuthorizationZone { get; set; }
        public AccountLevel AccountLevelRestriction { get; set; }
        public bool IsPaymentArea { get; set; }

        // Step Execution Results
        public SecurityResponseCode SecurityResponseCode { get; set; }
        public bool IsExecutionStateValid { get; set; }
    }
}
