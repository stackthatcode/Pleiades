using System;
using System.Web;
using Pleiades.Security;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Model
{
    public class SystemAuthorizationContextBase : ISystemAuthorizationContext
    {
        public SystemAuthorizationContextBase(HttpContextBase httpContext)
        {
            this.HttpContext = httpContext;
            this.SecurityResponseCode = SecurityResponseCode.Allowed;
            this.IsExecutionStateValid = true;
        }

        // Http 
        public HttpContextBase HttpContext { get; set; }

        // Identity-based System-level Authorization Requirements
        public AuthorizationZone AuthorizationZone { get; set; }
        public AccountLevel AccountLevelRestriction { get; set; }
        public bool IsPaymentArea { get; set; }

        // User
        public AggregateUser ThisUser { get; set; }
        public IdentityUser CurrentIdentity { get { return ThisUser == null ? null : ThisUser.IdentityUser; } }

        // Step Execution Results
        public SecurityResponseCode SecurityResponseCode { get; set; }
        public bool IsExecutionStateValid { get; set; }
    }
}