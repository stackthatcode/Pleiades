using System;
using Pleiades.Framework.Identity.Model;

namespace Pleiades.Framework.UnitTests.Identity.Execution
{
    public class StubUserGenerator
    {
        public static IdentityUser AnonymousUser
        {
            get
            {
                return new IdentityUser()
                {
                    UserRole = UserRole.Anonymous,
                };
            }
        }

        public static IdentityUser ActiveTrustedGoldUser
        {
            get
            {
                return new IdentityUser()
                {
                    UserRole = UserRole.Trusted,
                    AccountStatus = AccountStatus.Active,
                    AccountLevel = AccountLevel.Gold,
                };
            }
        }

        public static IdentityUser ActiveTrustedStandardUser
        {
            get
            {
                return new IdentityUser()
                {
                    UserRole = UserRole.Trusted,
                    AccountStatus = AccountStatus.Active,
                    AccountLevel = AccountLevel.Standard,
                };
            }
        }

        public static IdentityUser DelinquentTrustedUser
        {
            get
            {
                return new IdentityUser()
                {
                    UserRole = UserRole.Trusted,
                    AccountStatus = AccountStatus.PaymentRequired,
                    AccountLevel = AccountLevel.Gold,
                };
            }
        }

        public static IdentityUser DisabledAccountTrustedUser
        {
            get
            {
                return new IdentityUser()
                {
                    UserRole = UserRole.Trusted,
                    AccountStatus = AccountStatus.Disabled,
                    AccountLevel = AccountLevel.Gold,
                };
            }
        }

        public static IdentityUser AdminUser
        {
            get
            {
                return new IdentityUser()
                {
                    UserRole = UserRole.Admin,
                    AccountStatus = AccountStatus.Active,
                };
            }
        }

        public static IdentityUser SupremeUser
        {
            get
            {
                return new IdentityUser()
                {
                    UserRole = UserRole.Supreme,
                    AccountStatus = AccountStatus.Active,
                };
            }
        }
    }
}
