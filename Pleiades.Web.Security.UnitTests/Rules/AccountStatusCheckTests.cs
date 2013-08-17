using NUnit.Framework;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Rules;

namespace Pleiades.Web.Security.UnitTests.Rules
{
    [TestFixture]
    public class AccountStatusAuthorizationStepTests
    {
        [Test]
        public void DelinquentUser_Allowed_In_Public_Area()
        {
            // Arrange
            var context = AuthContextGenerator.PublicArea(StubUserGenerator.DelinquentTrustedUser);

            // Act
            context.AccountStatusCheck();
            
            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void DelinquentUser_NotAllowed_In_Restricted_Gold_Area()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.DelinquentTrustedUser);

            // Act
            context.AccountStatusCheck();
            
            // Assert
            Assert.AreEqual(SecurityCode.AccountDisabledNonPayment, context.SecurityCode);
            Assert.AreEqual(false, context.Pass);
        }

        [Test]
        public void DelinquentUser_NotAllowed_In_Restricted_Standard_Area()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.DelinquentTrustedUser);

            // Act
            context.AccountStatusCheck();
            
            // Assert
            Assert.AreEqual(SecurityCode.AccountDisabledNonPayment, context.SecurityCode);
            Assert.AreEqual(false, context.Pass);
        }

        [Test]
        public void DelinquentUser_Allowed_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedPaymentArea(StubUserGenerator.DelinquentTrustedUser);

            // Act
            context.AccountStatusCheck();
            
            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }



        [Test]
        public void DisabledAccount_Allowed_In_Public()
        {
            // Arrange
            var context = AuthContextGenerator.PublicArea(StubUserGenerator.DisabledAccountTrustedUser);

            // Act
            context.AccountStatusCheck();
            
            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void DisabledAccount_NotAllowed_In_RestrictedGoldArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.DisabledAccountTrustedUser);

            // Act
            context.AccountStatusCheck();

            // Assert
            Assert.AreEqual(SecurityCode.AccessDeniedSolicitLogon, context.SecurityCode);
            Assert.AreEqual(false, context.Pass);
        }

        [Test]
        public void DisabledAccount_NotAllowed_In_RestrictedStandardArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.DisabledAccountTrustedUser);

            // Act
            context.AccountStatusCheck();

            // Assert
            Assert.AreEqual(SecurityCode.AccessDeniedSolicitLogon, context.SecurityCode);
            Assert.AreEqual(false, context.Pass);
        }

        [Test]
        public void DisabledAccount_NotAllowed_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedPaymentArea(StubUserGenerator.DisabledAccountTrustedUser);

            // Act
            context.AccountStatusCheck();

            // Assert
            Assert.AreEqual(SecurityCode.AccessDeniedSolicitLogon, context.SecurityCode);
            Assert.AreEqual(false, context.Pass);
        }
    }
}