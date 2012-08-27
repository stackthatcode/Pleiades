using Pleiades.Security;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Model
{
    public class SystemAuthorizationContext : ISystemAuthorizationContext
    {
        public IdentityUser CurrentIdentity { get; set; }

        public AuthorizationZone AuthorizationZone { get; set; }
        public AccountLevel AccountLevelRestriction { get; set; }
        public bool IsPaymentArea { get; set; }
        public int? ResourceOwnerId { get; set; }

        public SecurityResponseCode SecurityResponseCode { get; set; }
        public bool IsExecutionStateValid { get; set; }


        public SystemAuthorizationContext()
        {
            this.CurrentIdentity = null;

            this.AuthorizationZone = AuthorizationZone.Public;
            this.AccountLevelRestriction = AccountLevel.Standard;
            this.IsPaymentArea = false;
            this.ResourceOwnerId = null;

            this.SecurityResponseCode = SecurityResponseCode.Allowed;
            this.IsExecutionStateValid = true;
        }       
    }
}