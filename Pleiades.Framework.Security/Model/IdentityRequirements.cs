namespace Pleiades.Framework.Identity.Model
{
    public class IdentityRequirements
    {
        public int? ResourceOwnerIdentityUserId { get; set; }
        public AuthorizationZone AuthorizationZone { get; set; }
        public AccountLevel AccountLevelRestriction { get; set; }
        public bool PaymentArea { get; set; }

        public IdentityRequirements()
        {
            this.ResourceOwnerIdentityUserId = null;
            this.AuthorizationZone = AuthorizationZone.Public;
            this.AccountLevelRestriction = AccountLevel.Standard;
            this.PaymentArea = false;
        }
    }
}