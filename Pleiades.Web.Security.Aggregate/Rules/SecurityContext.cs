using System.Web;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Rules
{
    public class SecurityContext
    {
        // Input
        public AggregateUser User { get; set; }
        public AuthorizationZone AuthorizationZone { get; set; }
        public AccountLevel AccountLevelRestriction { get; set; }
        public bool IsPaymentArea { get; set; }

        // Output
        public SecurityCode SecurityCode { get; set; }  
        public bool Pass { get; private set; }        
        
        public SecurityContext(AggregateUser user)
        {
            this.User = user;
            this.AuthorizationZone = AuthorizationZone.Public;
            this.AccountLevelRestriction = AccountLevel.Standard;
            this.IsPaymentArea = false;

            this.SecurityCode = SecurityCode.Allowed;
            this.Pass = true;
        }

        public SecurityContext Fail(SecurityCode securityCode)
        {
            this.Pass = false;
            this.SecurityCode = securityCode;
            return this;
        }
    }
}