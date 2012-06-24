using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Identity.Execution;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Security;

namespace Pleiades.Framework.UnitTests.Identity.Execution
{
    [TestFixture]
    public class AccountStatusAuthorizationStepTests
    {
        [Test]
        public void DelinquentUser_Allowed_In_Public_Area()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.DelinquentTrustedUser,
                IdentityRequirements = IdentityRequirementsGenerator.PublicArea,
            };

            var step = new AccountStatusAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);
            
            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void DelinquentUser_NotAllowed_In_Restricted_Gold_Area()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.DelinquentTrustedUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedGoldNonPaymentArea,
            };

            var step = new AccountStatusAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);
            
            // Assert
            Assert.AreEqual(SecurityResponseCode.AccountDisabledNonPayment, context.SecurityResponseCode);
            Assert.AreEqual(false, context.ExecutionStateValid);
        }

        [Test]
        public void DelinquentUser_NotAllowed_In_Restricted_Standard_Area()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.DelinquentTrustedUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedStandardNonPaymentArea,
            };

            var step = new AccountStatusAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);
            
            // Assert
            Assert.AreEqual(SecurityResponseCode.AccountDisabledNonPayment, context.SecurityResponseCode);
            Assert.AreEqual(false, context.ExecutionStateValid);
        }

        [Test]
        public void DelinquentUser_Allowed_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.DelinquentTrustedUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedPaymentArea,
            };

            var step = new AccountStatusAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);
            
            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }



        [Test]
        public void DisabledAccount_Allowed_In_Public()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.DisabledAccountTrustedUser,
                IdentityRequirements = IdentityRequirementsGenerator.PublicArea,
            };

            var step = new AccountStatusAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);
            
            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void DisabledAccount_NotAllowed_In_RestrictedGoldArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.DisabledAccountTrustedUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedGoldNonPaymentArea,
            };

            var step = new AccountStatusAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDeniedSolicitLogon, context.SecurityResponseCode);
            Assert.AreEqual(false, context.ExecutionStateValid);
        }

        [Test]
        public void DisabledAccount_NotAllowed_In_RestrictedStandardArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.DisabledAccountTrustedUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedStandardNonPaymentArea,
            };

            var step = new AccountStatusAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDeniedSolicitLogon, context.SecurityResponseCode);
            Assert.AreEqual(false, context.ExecutionStateValid);
        }

        [Test]
        public void DisabledAccount_NotAllowed_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.DisabledAccountTrustedUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedPaymentArea,
            };

            var step = new AccountStatusAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDeniedSolicitLogon, context.SecurityResponseCode);
            Assert.AreEqual(false, context.ExecutionStateValid);
        }
    }
}