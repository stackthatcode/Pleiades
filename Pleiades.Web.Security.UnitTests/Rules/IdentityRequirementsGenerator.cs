using Pleiades.Web.Security.Rules;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.UnitTests.Rules
{
    public class AuthContextGenerator
    {
        public static SecurityContext PublicArea(IdentityProfile user)
        {
            return new SecurityContext(null)
            {
                AuthorizationZone = AuthorizationZone.Public,
                User = new AggregateUser
                {
                    IdentityProfile = user,
                }
            };
        }

        public static SecurityContext RestrictedStandardNonPaymentArea(IdentityProfile user)
        {
            return new SecurityContext(null)
            { 
                AuthorizationZone = AuthorizationZone.Restricted,
                AccountLevelRestriction = AccountLevel.Standard,
                IsPaymentArea = false,
                User = new AggregateUser
                {
                    IdentityProfile = user,
                }
            };
        }

        public static SecurityContext RestrictedGoldNonPaymentArea(IdentityProfile user)
        {
            return new SecurityContext(null)
            {
                AuthorizationZone = AuthorizationZone.Restricted,
                AccountLevelRestriction = AccountLevel.Gold,
                IsPaymentArea = false,
                User = new AggregateUser
                {
                    IdentityProfile = user,
                }
            };
        }

        public static SecurityContext RestrictedPaymentArea(IdentityProfile user)
        {
            return new SecurityContext(null)
            {
                AuthorizationZone = AuthorizationZone.Restricted,
                AccountLevelRestriction = AccountLevel.Standard,
                IsPaymentArea = true,
                User = new AggregateUser
                {
                    IdentityProfile = user,
                }
            };
        }

        public static SecurityContext AdministrativeArea(IdentityProfile user)
        {
            return new SecurityContext(null)
            {
                AuthorizationZone = AuthorizationZone.Administrative,
                User = new AggregateUser
                {
                    IdentityProfile = user,
                }
            };
        }
    }
}
