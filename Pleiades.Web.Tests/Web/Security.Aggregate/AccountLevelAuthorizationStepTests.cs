using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Execution;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Model;
using Pleiades.Security;

namespace Pleiades.UnitTests.Identity.Execution
{
    [TestFixture]
    public class AccountLevelAuthorizationStepTests
    {
        [Test]
        public void ActiveTrustedStandardUser_Allowed_In_RestrictedStandardNonPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.ActiveTrustedStandardUser);

            var step = new AccountLevelAuthorizationStep();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void ActiveTrustedStandardUser_Denied_In_RestrictedGoldNonPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.ActiveTrustedStandardUser);

            var step = new AccountLevelAuthorizationStep();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDeniedToAccountLevel, context.SecurityResponseCode);
            Assert.AreEqual(false, context.IsExecutionStateValid);
        }



        [Test]
        public void ActiveTrustedGoldUser_Allowed_In_RestrictedStandardNonPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.ActiveTrustedGoldUser);

            var step = new AccountLevelAuthorizationStep();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void ActiveTrustedGoldUser_Allowed_In_RestrictedGoldNonPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.ActiveTrustedGoldUser);
            var step = new AccountLevelAuthorizationStep();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

    }
}