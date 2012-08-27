using Pleiades.Web.Security.Model;

namespace Pleiades.UnitTests.Identity.Execution
{
    public class AuthContextGenerator
    {
        public static SystemAuthorizationContext PublicArea(IdentityUser user)
        {
            return new SystemAuthorizationContext()
            {
                AuthorizationZone = AuthorizationZone.Public,
                CurrentIdentity = user,
            };
        }

        public static SystemAuthorizationContext RestrictedStandardNonPaymentArea(IdentityUser user)
        {
            return new SystemAuthorizationContext()
            {
                AuthorizationZone = AuthorizationZone.Restricted,
                AccountLevelRestriction = AccountLevel.Standard,
                IsPaymentArea = false,
                CurrentIdentity = user,
            };
        }

        public static SystemAuthorizationContext RestrictedGoldNonPaymentArea(IdentityUser user)
        {
            return new SystemAuthorizationContext()
            {
                AuthorizationZone = AuthorizationZone.Restricted,
                AccountLevelRestriction = AccountLevel.Gold,
                IsPaymentArea = false,
                CurrentIdentity = user,
            };
        }

        public static SystemAuthorizationContext RestrictedPaymentArea(IdentityUser user)
        {
            return new SystemAuthorizationContext()
            {
                AuthorizationZone = AuthorizationZone.Restricted,
                AccountLevelRestriction = AccountLevel.Standard,
                IsPaymentArea = true,
                CurrentIdentity = user,
            };
        }

        public static SystemAuthorizationContext AdministrativeArea(IdentityUser user)
        {
            return new SystemAuthorizationContext()
            {
                AuthorizationZone = AuthorizationZone.Administrative,
                CurrentIdentity = user,
            };
        }
    }
}
