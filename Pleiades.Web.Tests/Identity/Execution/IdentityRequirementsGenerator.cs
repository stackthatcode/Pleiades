using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.UnitTests.Identity.Execution
{
    public class AuthContextGenerator
    {
        public static IdentityAuthorizationContext PublicArea(IdentityUser user)
        {
            return new IdentityAuthorizationContext()
            {
                AuthorizationZone = AuthorizationZone.Public,
                CurrentUser = user,
            };
        }

        public static IdentityAuthorizationContext RestrictedStandardNonPaymentArea(IdentityUser user)
        {
            return new IdentityAuthorizationContext()
            {
                AuthorizationZone = AuthorizationZone.Restricted,
                AccountLevelRestriction = AccountLevel.Standard,
                IsPaymentArea = false,
                CurrentUser = user,
            };
        }

        public static IdentityAuthorizationContext RestrictedGoldNonPaymentArea(IdentityUser user)
        {
            return new IdentityAuthorizationContext()
            {
                AuthorizationZone = AuthorizationZone.Restricted,
                AccountLevelRestriction = AccountLevel.Gold,
                IsPaymentArea = false,
                CurrentUser = user,
            };
        }

        public static IdentityAuthorizationContext RestrictedPaymentArea(IdentityUser user)
        {
            return new IdentityAuthorizationContext()
            {
                AuthorizationZone = AuthorizationZone.Restricted,
                AccountLevelRestriction = AccountLevel.Standard,
                IsPaymentArea = true,
                CurrentUser = user,
            };
        }

        public static IdentityAuthorizationContext AdministrativeArea(IdentityUser user)
        {
            return new IdentityAuthorizationContext()
            {
                AuthorizationZone = AuthorizationZone.Administrative,
                CurrentUser = user,
            };
        }
    }
}
