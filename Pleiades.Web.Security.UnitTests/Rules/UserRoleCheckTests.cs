using NUnit.Framework;
using Pleiades.Web.Security.Rules;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.UnitTests.Rules
{
    [TestFixture]
    public class RoleAuthorizationStepTests
    {
        [Test]
        public void AnonymousUser_Allowed_In_Public_Area()
        {
            // Arrange
            var context = AuthContextGenerator.PublicArea(StubUserGenerator.AnonymousUser);

            // Act
            context.UserRoleCheck();
            
            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void AnonymousUser_Denied_In_RestrictedStandardArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.AnonymousUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.AccessDeniedSolicitLogon, context.SecurityCode);
            Assert.AreEqual(false, context.Pass);
        }

        [Test]
        public void AnonymousUser_Denied_In_RestrictedGoldArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.AnonymousUser);
            
            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.AccessDeniedSolicitLogon, context.SecurityCode);
            Assert.AreEqual(false, context.Pass);
        }

        [Test]
        public void AnonymousUser_Denied_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedPaymentArea(StubUserGenerator.AnonymousUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.AccessDeniedSolicitLogon, context.SecurityCode);
            Assert.AreEqual(false, context.Pass);
        }

        [Test]
        public void AnonymousUser_Denied_In_AdministrativeArea()
        {
            // Arrange
            var context = AuthContextGenerator.AdministrativeArea(StubUserGenerator.AnonymousUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.AccessDeniedSolicitLogon, context.SecurityCode);
            Assert.AreEqual(false, context.Pass);
        }



        [Test]
        public void TrustedUser_Allowed_In_Public_Area()
        {
            // Arrange
            var context = AuthContextGenerator.PublicArea(StubUserGenerator.ActiveTrustedStandardUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void TrustedUser_Allowed_In_RestrictedStandardArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.ActiveTrustedStandardUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void TrustedUser_Allowed_In_RestrictedGoldArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.ActiveTrustedStandardUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void TrustedUser_Allowed_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedPaymentArea(StubUserGenerator.ActiveTrustedStandardUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void TrustedUser_Denied_In_AdministrativeArea()
        {
            // Arrange
            var context = AuthContextGenerator.AdministrativeArea(StubUserGenerator.ActiveTrustedStandardUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.AccessDeniedSolicitLogon, context.SecurityCode);
            Assert.AreEqual(false, context.Pass);
        }



        [Test]
        public void AdminUser_Allowed_In_Public_Area()
        {
            // Arrange
            var context = AuthContextGenerator.PublicArea(StubUserGenerator.AdminUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void AdminUser_Allowed_In_RestrictedStandardArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.AdminUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void AdminUser_Allowed_In_RestrictedGoldArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.AdminUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void AdminUser_Allowed_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedPaymentArea(StubUserGenerator.AdminUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void AdminUser_Allowed_In_AdministrativeArea()
        {
            // Arrange
            var context = AuthContextGenerator.AdministrativeArea(StubUserGenerator.AdminUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }



        [Test]
        public void SupremeUser_Allowed_In_Public_Area()
        {
            // Arrange
            var context = AuthContextGenerator.PublicArea(StubUserGenerator.SupremeUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void SupremeUser_Allowed_In_RestrictedStandardArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.SupremeUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void SupremeUser_Allowed_In_RestrictedGoldArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.SupremeUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void SupremeUser_Allowed_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedPaymentArea(StubUserGenerator.SupremeUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void SupremeUser_Allowed_In_AdministrativeArea()
        {
            // Arrange
            var context = AuthContextGenerator.AdministrativeArea(StubUserGenerator.SupremeUser);

            // Act
            context.UserRoleCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }
    }
}