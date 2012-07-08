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
    public class AccountLevelAuthorizationStepTests
    {
        [Test]
        public void ActiveTrustedStandardUser_Allowed_In_RestrictedStandardNonPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.ActiveTrustedStandardUser);

            var step = new AccountLevelAuthorizationStep<IdentityAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void ActiveTrustedStandardUser_Denied_In_RestrictedGoldNonPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.ActiveTrustedStandardUser);

            var step = new AccountLevelAuthorizationStep<IdentityAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDeniedToAccountLevel, context.SecurityResponseCode);
            Assert.AreEqual(false, context.ExecutionStateValid);
        }



        [Test]
        public void ActiveTrustedGoldUser_Allowed_In_RestrictedStandardNonPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.ActiveTrustedGoldUser);

            var step = new AccountLevelAuthorizationStep<IdentityAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void ActiveTrustedGoldUser_Allowed_In_RestrictedGoldNonPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.ActiveTrustedGoldUser);
            var step = new AccountLevelAuthorizationStep<IdentityAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

    }
}