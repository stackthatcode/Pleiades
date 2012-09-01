using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Model;

namespace Pleiades.UnitTests.Identity.Execution
{
    public class AuthContextGenerator
    {
        public static SystemAuthorizationContext PublicArea(IdentityUser user)
        {
            return new SystemAuthorizationContext(null)
            {
                AuthorizationZone = AuthorizationZone.Public,
                ThisUser = new AggregateUser
                {
                    Identity = user,
                }
            };
        }

        public static SystemAuthorizationContext RestrictedStandardNonPaymentArea(IdentityUser user)
        {
            return new SystemAuthorizationContext(null)
            {
                AuthorizationZone = AuthorizationZone.Restricted,
                AccountLevelRestriction = AccountLevel.Standard,
                IsPaymentArea = false,
                ThisUser = new AggregateUser
                {
                    Identity = user,
                }
            };
        }

        public static SystemAuthorizationContext RestrictedGoldNonPaymentArea(IdentityUser user)
        {
            return new SystemAuthorizationContext(null)
            {
                AuthorizationZone = AuthorizationZone.Restricted,
                AccountLevelRestriction = AccountLevel.Gold,
                IsPaymentArea = false,
                ThisUser = new AggregateUser
                {
                    Identity = user,
                }
            };
        }

        public static SystemAuthorizationContext RestrictedPaymentArea(IdentityUser user)
        {
            return new SystemAuthorizationContext(null)
            {
                AuthorizationZone = AuthorizationZone.Restricted,
                AccountLevelRestriction = AccountLevel.Standard,
                IsPaymentArea = true,
                ThisUser = new AggregateUser
                {
                    Identity = user,
                }
            };
        }

        public static SystemAuthorizationContext AdministrativeArea(IdentityUser user)
        {
            return new SystemAuthorizationContext(null)
            {
                AuthorizationZone = AuthorizationZone.Administrative,
                ThisUser = new AggregateUser
                {
                    Identity = user,
                }
            };
        }
    }
}
