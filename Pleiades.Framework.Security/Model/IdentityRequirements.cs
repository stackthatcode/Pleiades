namespace Pleiades.Framework.Identity.Model
{
    public class IdentityRequirements
    {
        public AuthorizationZone AuthorizationZone { get; set; }
        public AccountLevel AccountLevelRestriction { get; set; }
        public bool PaymentArea { get; set; }
        public int? ResourceOwnerIdentityUserId { get; set; }

        public IdentityRequirements()
        {
            this.AuthorizationZone = AuthorizationZone.Public;
            this.AccountLevelRestriction = AccountLevel.Standard;
            this.PaymentArea = false;
            this.ResourceOwnerIdentityUserId = null;
        }
    }
}