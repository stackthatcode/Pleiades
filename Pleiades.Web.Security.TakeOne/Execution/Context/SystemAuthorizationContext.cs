using System.Web;
using Pleiades.Security;
using Pleiades.Execution;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Execution.Context
{
    public class SystemAuthorizationContext : ISecurityContext, IStepContext
    {
        public HttpContextBase HttpContext { get; set; }
        public AggregateUser ThisUser { get; set; }

        public AuthorizationZone AuthorizationZone { get; set; }
        public AccountLevel AccountLevelRestriction { get; set; }
        public bool IsPaymentArea { get; set; }

        public SecurityResponseCode SecurityResponseCode { get; set; }  // ISecurityContext
        public bool IsExecutionStateValid { get; set; }                 // IStepContext
        
        
        public SystemAuthorizationContext(HttpContextBase httpContext)
        {
            this.HttpContext = httpContext;            
            this.ThisUser = null;

            this.AuthorizationZone = AuthorizationZone.Public;
            this.AccountLevelRestriction = AccountLevel.Standard;
            this.IsPaymentArea = false;

            this.SecurityResponseCode = SecurityResponseCode.Allowed;
            this.IsExecutionStateValid = true;
        }       
    }
}