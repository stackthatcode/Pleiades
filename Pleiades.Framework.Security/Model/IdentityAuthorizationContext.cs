using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Security;

namespace Pleiades.Framework.Identity.Model
{
    public class IdentityAuthorizationContext : IIdentityAuthorizationContext
    {
        public IdentityUser CurrentUser { get; set; }

        public AuthorizationZone AuthorizationZone { get; set; }
        public AccountLevel AccountLevelRestriction { get; set; }
        public bool IsPaymentArea { get; set; }
        public int? ResourceOwnerId { get; set; }

        public SecurityResponseCode SecurityResponseCode { get; set; }
        public bool ExecutionStateValid { get; set; }


        public IdentityAuthorizationContext()
        {
            this.CurrentUser = null;

            this.AuthorizationZone = AuthorizationZone.Public;
            this.AccountLevelRestriction = AccountLevel.Standard;
            this.IsPaymentArea = false;
            this.ResourceOwnerId = null;

            this.SecurityResponseCode = Security.SecurityResponseCode.Allowed;
            this.ExecutionStateValid = true;
        }       
    }
}