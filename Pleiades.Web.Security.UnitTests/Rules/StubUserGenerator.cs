using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.UnitTests.Rules
{
    public class StubUserGenerator
    {
        public static IdentityProfile AnonymousUser
        {
            get
            {
                return new IdentityProfile()
                {
                    UserRole = UserRole.Anonymous,
                };
            }
        }

        public static IdentityProfile ActiveTrustedGoldUser
        {
            get
            {
                return new IdentityProfile()
                {
                    UserRole = UserRole.Trusted,
                    AccountStatus = AccountStatus.Active,
                    AccountLevel = AccountLevel.Gold,
                };
            }
        }

        public static IdentityProfile ActiveTrustedStandardUser
        {
            get
            {
                return new IdentityProfile()
                {
                    UserRole = UserRole.Trusted,
                    AccountStatus = AccountStatus.Active,
                    AccountLevel = AccountLevel.Standard,
                };
            }
        }

        public static IdentityProfile DelinquentTrustedUser
        {
            get
            {
                return new IdentityProfile()
                {
                    UserRole = UserRole.Trusted,
                    AccountStatus = AccountStatus.PaymentRequired,
                    AccountLevel = AccountLevel.Gold,
                };
            }
        }

        public static IdentityProfile DisabledAccountTrustedUser
        {
            get
            {
                return new IdentityProfile()
                {
                    UserRole = UserRole.Trusted,
                    AccountStatus = AccountStatus.Disabled,
                    AccountLevel = AccountLevel.Gold,
                };
            }
        }

        public static IdentityProfile AdminUser
        {
            get
            {
                return new IdentityProfile()
                {
                    UserRole = UserRole.Admin,
                    AccountStatus = AccountStatus.Active,
                };
            }
        }

        public static IdentityProfile SupremeUser
        {
            get
            {
                return new IdentityProfile()
                {
                    UserRole = UserRole.Supreme,
                    AccountStatus = AccountStatus.Active,
                };
            }
        }
    }
}
