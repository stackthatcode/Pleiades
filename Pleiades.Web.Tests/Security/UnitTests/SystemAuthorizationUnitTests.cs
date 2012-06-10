using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Gallio.Framework;
using MbUnit.Framework;
using Rhino.Mocks;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Model;
using Pleiades.Framework.Web.Security.Concrete;
using Pleiades.Framework.TestHelpers;
using Pleiades.Framework.TestHelpers.Web;

namespace Pleiades.Web.Tests.SecurityUnitTests
{
    [TestFixture]
    public class SystemAuthorizationUnitTests
    {
        ISystemAuthorizationService systemauthservice = new SystemAuthService();

        #region User Generators
        public static DomainUser AnonymousUser
        {
            get
            {
                return new DomainUser()
                {
                    UserRole = UserRole.Anonymous,
                };
            }
        }

        public static DomainUser ActiveTrustedGoldUser
        {
            get
            {
                return new DomainUser()
                {
                    UserRole = UserRole.Trusted,
                    AccountStatus = AccountStatus.Active,
                    AccountLevel = AccountLevel.Gold,
                };
            }
        }

        public static DomainUser ActiveTrustedStandardUser
        {
            get
            {
                return new DomainUser()
                {
                    UserRole = UserRole.Trusted,
                    AccountStatus = AccountStatus.Active,
                    AccountLevel = AccountLevel.Standard,
                };
            }
        }

        public static DomainUser DelinquentTrustedUser
        {
            get
            {
                return new DomainUser()
                {
                    UserRole = UserRole.Trusted,
                    AccountStatus = AccountStatus.PaymentRequired,
                    AccountLevel = AccountLevel.Gold,
                };
            }
        }

        public static DomainUser DisabledAccountTrustedUser
        {
            get
            {
                return new DomainUser()
                {
                    UserRole = UserRole.Trusted,
                    AccountStatus = AccountStatus.Disabled,
                    AccountLevel = AccountLevel.Gold,
                };
            }
        }

        public static DomainUser AdminUser
        {
            get
            {
                return new DomainUser()
                {
                    UserRole = UserRole.Admin,
                    AccountStatus = AccountStatus.Active,
                };
            }
        }

        public static DomainUser RootUser
        {
            get
            {
                return new DomainUser()
                {
                    UserRole = UserRole.Root,
                    AccountStatus = AccountStatus.Active,
                };
            }
        }
        #endregion

        #region Security Requirement Context
        public SecurityRequirementsContext PublicContext
        {
            get
            {
                return new SecurityRequirementsContext()
                {
                    AuthorizationZone = AuthorizationZone.Public,
                };
            }
        }

        public SecurityRequirementsContext RestrictedStandardContextNonPaymentArea
        {
            get
            {
                return new SecurityRequirementsContext()
                {
                    AuthorizationZone = AuthorizationZone.Restricted,
                    AccountLevelRestriction = AccountLevel.Standard,
                    PaymentArea = false,
                };
            }
        }

        public SecurityRequirementsContext RestrictedGoldContextNonPaymentArea
        {
            get
            {
                return new SecurityRequirementsContext()
                {
                    AuthorizationZone = AuthorizationZone.Restricted,
                    AccountLevelRestriction = AccountLevel.Gold,
                    PaymentArea = false,
                };
            }
        }

        public SecurityRequirementsContext RestrictedStandardPaymentArea
        {
            get
            {
                return new SecurityRequirementsContext()
                {
                    AuthorizationZone = AuthorizationZone.Restricted,
                    AccountLevelRestriction = AccountLevel.Standard,
                    PaymentArea = true,
                };
            }
        }

        public SecurityRequirementsContext AdministrativeContext
        {
            get
            {
                return new SecurityRequirementsContext()
                {
                    AuthorizationZone = AuthorizationZone.Administrative,
                };
            }
        }
        #endregion

        #region Role Authorization Tests
        [Test]
        public void AnonymousUserCanOnlyAccessPublicAreas()
        {
            var authcode1 = systemauthservice.Authorize(AnonymousUser, this.PublicContext);
            authcode1.ShouldEqual(SecurityResponseCode.Allowed);

            var authcode2 = systemauthservice.Authorize(AnonymousUser, this.RestrictedStandardContextNonPaymentArea);
            authcode2.ShouldEqual(SecurityResponseCode.AccessDeniedSolicitLogon);

            var authcode3a = systemauthservice.Authorize(AnonymousUser, this.RestrictedGoldContextNonPaymentArea);
            authcode3a.ShouldEqual(SecurityResponseCode.AccessDeniedSolicitLogon);

            var authcode3b = systemauthservice.Authorize(AnonymousUser, this.RestrictedStandardPaymentArea);
            authcode3b.ShouldEqual(SecurityResponseCode.AccessDeniedSolicitLogon);

            var authcode4 = systemauthservice.Authorize(AnonymousUser, this.AdministrativeContext);
            authcode4.ShouldEqual(SecurityResponseCode.AccessDenied);
        }

        [Test]
        public void TrustUserCanAccessPublicAndRestrictedAreas()
        {
            var authcode1 = systemauthservice.Authorize(ActiveTrustedGoldUser, this.PublicContext);
            authcode1.ShouldEqual(SecurityResponseCode.Allowed);

            var authcode2 = systemauthservice.Authorize(ActiveTrustedGoldUser, this.RestrictedStandardContextNonPaymentArea);
            authcode2.ShouldEqual(SecurityResponseCode.Allowed);

            var authcode3 = systemauthservice.Authorize(ActiveTrustedGoldUser, this.RestrictedGoldContextNonPaymentArea);
            authcode3.ShouldEqual(SecurityResponseCode.Allowed);

            var authcode3b = systemauthservice.Authorize(ActiveTrustedGoldUser, this.RestrictedStandardPaymentArea);
            authcode3b.ShouldEqual(SecurityResponseCode.Allowed);

            var authcode4 = systemauthservice.Authorize(ActiveTrustedGoldUser, this.AdministrativeContext);
            authcode4.ShouldEqual(SecurityResponseCode.AccessDenied);
        }

        [Test]
        public void AdminUserCanAccessAnything()
        {
            var authcode1 = systemauthservice.Authorize(AdminUser, this.PublicContext);
            authcode1.ShouldEqual(SecurityResponseCode.Allowed);

            var authcode2 = systemauthservice.Authorize(AdminUser, this.RestrictedStandardContextNonPaymentArea);
            authcode2.ShouldEqual(SecurityResponseCode.Allowed);

            var authcode3 = systemauthservice.Authorize(AdminUser, this.RestrictedGoldContextNonPaymentArea);
            authcode3.ShouldEqual(SecurityResponseCode.Allowed);

            var authcode3b = systemauthservice.Authorize(AdminUser, this.RestrictedStandardPaymentArea);
            authcode3b.ShouldEqual(SecurityResponseCode.Allowed);

            var authcode4 = systemauthservice.Authorize(AdminUser, this.AdministrativeContext);
            authcode4.ShouldEqual(SecurityResponseCode.Allowed);
        }

        [Test]
        public void RootUserCanAccessAnything()
        {
            var authcode1 = systemauthservice.Authorize(RootUser, this.PublicContext);
            authcode1.ShouldEqual(SecurityResponseCode.Allowed);

            var authcode2 = systemauthservice.Authorize(RootUser, this.RestrictedStandardContextNonPaymentArea);
            authcode2.ShouldEqual(SecurityResponseCode.Allowed);

            var authcode3 = systemauthservice.Authorize(RootUser, this.RestrictedGoldContextNonPaymentArea);
            authcode3.ShouldEqual(SecurityResponseCode.Allowed);

            var authcode3b = systemauthservice.Authorize(RootUser, this.RestrictedStandardPaymentArea);
            authcode3b.ShouldEqual(SecurityResponseCode.Allowed);

            var authcode4 = systemauthservice.Authorize(RootUser, this.AdministrativeContext);
            authcode4.ShouldEqual(SecurityResponseCode.Allowed);
        }
        #endregion

        #region Status Authorization Tests
        [Test]
        public void DelinquentUserDeniedNonPaymentAreaAndAllowedPaymentArea()
        {
            var authcode1 = systemauthservice.Authorize(DelinquentTrustedUser, this.PublicContext);
            authcode1.ShouldEqual(SecurityResponseCode.Allowed);

            var authcode2 = systemauthservice.Authorize(DelinquentTrustedUser, this.RestrictedGoldContextNonPaymentArea);
            authcode2.ShouldEqual(SecurityResponseCode.AccountDisabledNonPayment);

            var authcode3 = systemauthservice.Authorize(DelinquentTrustedUser, this.RestrictedStandardContextNonPaymentArea);
            authcode3.ShouldEqual(SecurityResponseCode.AccountDisabledNonPayment);

            var authcode4 = systemauthservice.Authorize(DelinquentTrustedUser, this.RestrictedStandardPaymentArea);
            authcode4.ShouldEqual(SecurityResponseCode.Allowed);
        }

        [Test]
        public void DisabledAccountNotAllowedOnRestricted()
        {
            var authcode1 = systemauthservice.Authorize(DisabledAccountTrustedUser, this.PublicContext);
            authcode1.ShouldEqual(SecurityResponseCode.Allowed);

            var authcode2 = systemauthservice.Authorize(DisabledAccountTrustedUser, this.RestrictedGoldContextNonPaymentArea);
            authcode2.ShouldEqual(SecurityResponseCode.AccessDeniedSolicitLogon);

            var authcode3 = systemauthservice.Authorize(DisabledAccountTrustedUser, this.RestrictedStandardContextNonPaymentArea);
            authcode3.ShouldEqual(SecurityResponseCode.AccessDeniedSolicitLogon);

            var authcode4 = systemauthservice.Authorize(DisabledAccountTrustedUser, this.RestrictedStandardPaymentArea);
            authcode4.ShouldEqual(SecurityResponseCode.AccessDeniedSolicitLogon);
        }
        #endregion

        #region Account Level Unit Tests
        [Test]
        public void GoldAndStandardAccountsCanAccessRestrictedStandard()
        {
            var authcode1 = systemauthservice.Authorize(ActiveTrustedStandardUser, this.RestrictedStandardContextNonPaymentArea);
            authcode1.ShouldEqual(SecurityResponseCode.Allowed);

            var authcode2 = systemauthservice.Authorize(ActiveTrustedGoldUser, this.RestrictedStandardContextNonPaymentArea);
            authcode2.ShouldEqual(SecurityResponseCode.Allowed);
        }

        [Test]
        public void OnlyGoldAccountsCanAccessRestrictedGold()
        {
            var authcode1 = systemauthservice.Authorize(ActiveTrustedStandardUser, this.RestrictedGoldContextNonPaymentArea);
            authcode1.ShouldEqual(SecurityResponseCode.AccessDeniedToAccountLevel);
            
            var authcode2 = systemauthservice.Authorize(ActiveTrustedGoldUser, this.RestrictedGoldContextNonPaymentArea);
            authcode2.ShouldEqual(SecurityResponseCode.Allowed);
        }
        #endregion

    }
}
