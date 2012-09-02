using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Execution;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Model;
using Pleiades.Security;

namespace Pleiades.UnitTests.Web.Security.Aggregate.Step
{
    [TestFixture]
    public class AccountStatusAuthorizationStepTests
    {
        [Test]
        public void DelinquentUser_Allowed_In_Public_Area()
        {
            // Arrange
            var context = AuthContextGenerator.PublicArea(StubUserGenerator.DelinquentTrustedUser);
            var step = new AccountStatusAuthorizationStep();

            // Act
            step.Execute(context);
            
            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void DelinquentUser_NotAllowed_In_Restricted_Gold_Area()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.DelinquentTrustedUser);
            var step = new AccountStatusAuthorizationStep();

            // Act
            step.Execute(context);
            
            // Assert
            Assert.AreEqual(SecurityResponseCode.AccountDisabledNonPayment, context.SecurityResponseCode);
            Assert.AreEqual(false, context.IsExecutionStateValid);
        }

        [Test]
        public void DelinquentUser_NotAllowed_In_Restricted_Standard_Area()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.DelinquentTrustedUser);
            var step = new AccountStatusAuthorizationStep();

            // Act
            step.Execute(context);
            
            // Assert
            Assert.AreEqual(SecurityResponseCode.AccountDisabledNonPayment, context.SecurityResponseCode);
            Assert.AreEqual(false, context.IsExecutionStateValid);
        }

        [Test]
        public void DelinquentUser_Allowed_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedPaymentArea(StubUserGenerator.DelinquentTrustedUser);
            var step = new AccountStatusAuthorizationStep();

            // Act
            step.Execute(context);
            
            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }



        [Test]
        public void DisabledAccount_Allowed_In_Public()
        {
            // Arrange
            var context = AuthContextGenerator.PublicArea(StubUserGenerator.DisabledAccountTrustedUser);
            var step = new AccountStatusAuthorizationStep();

            // Act
            step.Execute(context);
            
            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void DisabledAccount_NotAllowed_In_RestrictedGoldArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.DisabledAccountTrustedUser);
            var step = new AccountStatusAuthorizationStep();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDeniedSolicitLogon, context.SecurityResponseCode);
            Assert.AreEqual(false, context.IsExecutionStateValid);
        }

        [Test]
        public void DisabledAccount_NotAllowed_In_RestrictedStandardArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.DisabledAccountTrustedUser);
            var step = new AccountStatusAuthorizationStep();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDeniedSolicitLogon, context.SecurityResponseCode);
            Assert.AreEqual(false, context.IsExecutionStateValid);
        }

        [Test]
        public void DisabledAccount_NotAllowed_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedPaymentArea(StubUserGenerator.DisabledAccountTrustedUser);
            var step = new AccountStatusAuthorizationStep();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDeniedSolicitLogon, context.SecurityResponseCode);
            Assert.AreEqual(false, context.IsExecutionStateValid);
        }
    }
}