using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Web.Security.Rules;
using Pleiades.Web.Security.Model;

namespace Pleiades.UnitTests.Web.Security.Aggregate.Step
{
    [TestFixture]
    public class AccountLevelAuthorizationStepTests
    {
        [Test]
        public void ActiveTrustedStandardUser_Allowed_In_RestrictedStandardNonPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.ActiveTrustedStandardUser);

            // Act
            context.AccountLevelCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void ActiveTrustedStandardUser_Denied_In_RestrictedGoldNonPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.ActiveTrustedStandardUser);

            // Act
            context.AccountLevelCheck();

            // Assert
            Assert.AreEqual(SecurityCode.AccessDeniedToAccountLevel, context.SecurityCode);
            Assert.AreEqual(false, context.Pass);
        }



        [Test]
        public void ActiveTrustedGoldUser_Allowed_In_RestrictedStandardNonPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.ActiveTrustedGoldUser);

            // Act
            context.AccountLevelCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

        [Test]
        public void ActiveTrustedGoldUser_Allowed_In_RestrictedGoldNonPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.ActiveTrustedGoldUser);

            // Act
            context.AccountLevelCheck();

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, context.SecurityCode);
            Assert.AreEqual(true, context.Pass);
        }

    }
}