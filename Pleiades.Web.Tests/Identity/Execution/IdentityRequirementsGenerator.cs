using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.UnitTests.Identity.Execution
{
    public class IdentityRequirementsGenerator
    {
        public static IdentityRequirements PublicArea
        {
            get
            {
                return new IdentityRequirements()
                {
                    AuthorizationZone = AuthorizationZone.Public,
                };
            }
        }

        public static IdentityRequirements RestrictedStandardNonPaymentArea
        {
            get
            {
                return new IdentityRequirements()
                {
                    AuthorizationZone = AuthorizationZone.Restricted,
                    AccountLevelRestriction = AccountLevel.Standard,
                    PaymentArea = false,
                };
            }
        }

        public static IdentityRequirements RestrictedGoldNonPaymentArea
        {
            get
            {
                return new IdentityRequirements()
                {
                    AuthorizationZone = AuthorizationZone.Restricted,
                    AccountLevelRestriction = AccountLevel.Gold,
                    PaymentArea = false,
                };
            }
        }

        public static IdentityRequirements RestrictedPaymentArea
        {
            get
            {
                return new IdentityRequirements()
                {
                    AuthorizationZone = AuthorizationZone.Restricted,
                    AccountLevelRestriction = AccountLevel.Standard,
                    PaymentArea = true,
                };
            }
        }

        public static IdentityRequirements AdministrativeArea
        {
            get
            {
                return new IdentityRequirements()
                {
                    AuthorizationZone = AuthorizationZone.Administrative,
                };
            }
        }
    }
}
