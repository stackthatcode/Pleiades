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
    public class RoleAuthorizationStepTests
    {
        [Test]
        public void AnonymousUser_Allowed_In_Public_Area()
        {
            // Arrange
            var context = AuthContextGenerator.PublicArea(StubUserGenerator.AnonymousUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);
            
            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void AnonymousUser_Denied_In_RestrictedStandardArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.AnonymousUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDeniedSolicitLogon, context.SecurityResponseCode);
            Assert.AreEqual(false, context.IsExecutionStateValid);
        }

        [Test]
        public void AnonymousUser_Denied_In_RestrictedGoldArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.AnonymousUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDeniedSolicitLogon, context.SecurityResponseCode);
            Assert.AreEqual(false, context.IsExecutionStateValid);
        }

        [Test]
        public void AnonymousUser_Denied_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedPaymentArea(StubUserGenerator.AnonymousUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDeniedSolicitLogon, context.SecurityResponseCode);
            Assert.AreEqual(false, context.IsExecutionStateValid);
        }

        [Test]
        public void AnonymousUser_Denied_In_AdministrativeArea()
        {
            // Arrange
            var context = AuthContextGenerator.AdministrativeArea(StubUserGenerator.AnonymousUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDenied, context.SecurityResponseCode);
            Assert.AreEqual(false, context.IsExecutionStateValid);
        }



        [Test]
        public void TrustedUser_Allowed_In_Public_Area()
        {
            // Arrange
            var context = AuthContextGenerator.PublicArea(StubUserGenerator.ActiveTrustedStandardUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void TrustedUser_Allowed_In_RestrictedStandardArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.ActiveTrustedStandardUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void TrustedUser_Allowed_In_RestrictedGoldArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.ActiveTrustedStandardUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void TrustedUser_Allowed_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedPaymentArea(StubUserGenerator.ActiveTrustedStandardUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void TrustedUser_Denied_In_AdministrativeArea()
        {
            // Arrange
            var context = AuthContextGenerator.AdministrativeArea(StubUserGenerator.ActiveTrustedStandardUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDenied, context.SecurityResponseCode);
            Assert.AreEqual(false, context.IsExecutionStateValid);
        }



        [Test]
        public void AdminUser_Allowed_In_Public_Area()
        {
            // Arrange
            var context = AuthContextGenerator.PublicArea(StubUserGenerator.AdminUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void AdminUser_Allowed_In_RestrictedStandardArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.AdminUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void AdminUser_Allowed_In_RestrictedGoldArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.AdminUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void AdminUser_Allowed_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedPaymentArea(StubUserGenerator.AdminUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void AdminUser_Allowed_In_AdministrativeArea()
        {
            // Arrange
            var context = AuthContextGenerator.AdministrativeArea(StubUserGenerator.AdminUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }



        [Test]
        public void SupremeUser_Allowed_In_Public_Area()
        {
            // Arrange
            var context = AuthContextGenerator.PublicArea(StubUserGenerator.SupremeUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void SupremeUser_Allowed_In_RestrictedStandardArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedStandardNonPaymentArea(StubUserGenerator.SupremeUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void SupremeUser_Allowed_In_RestrictedGoldArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedGoldNonPaymentArea(StubUserGenerator.SupremeUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void SupremeUser_Allowed_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = AuthContextGenerator.RestrictedPaymentArea(StubUserGenerator.SupremeUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void SupremeUser_Allowed_In_AdministrativeArea()
        {
            // Arrange
            var context = AuthContextGenerator.AdministrativeArea(StubUserGenerator.SupremeUser);
            var step = new RoleAuthorizationStep<SystemAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }
    }
}