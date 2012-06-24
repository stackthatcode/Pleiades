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
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.AnonymousUser,
                IdentityRequirements = IdentityRequirementsGenerator.PublicArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);
            
            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void AnonymousUser_Denied_In_RestrictedStandardArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.AnonymousUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedStandardNonPaymentArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDeniedSolicitLogon, context.SecurityResponseCode);
            Assert.AreEqual(false, context.ExecutionStateValid);
        }

        [Test]
        public void AnonymousUser_Denied_In_RestrictedGoldArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.AnonymousUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedGoldNonPaymentArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDeniedSolicitLogon, context.SecurityResponseCode);
            Assert.AreEqual(false, context.ExecutionStateValid);
        }

        [Test]
        public void AnonymousUser_Denied_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.AnonymousUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedPaymentArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDeniedSolicitLogon, context.SecurityResponseCode);
            Assert.AreEqual(false, context.ExecutionStateValid);
        }

        [Test]
        public void AnonymousUser_Denied_In_AdministrativeArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.AnonymousUser,
                IdentityRequirements = IdentityRequirementsGenerator.AdministrativeArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDenied, context.SecurityResponseCode);
            Assert.AreEqual(false, context.ExecutionStateValid);
        }



        [Test]
        public void TrustedUser_Allowed_In_Public_Area()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.ActiveTrustedStandardUser,
                IdentityRequirements = IdentityRequirementsGenerator.PublicArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void TrustedUser_Allowed_In_RestrictedStandardArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.ActiveTrustedStandardUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedStandardNonPaymentArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void TrustedUser_Allowed_In_RestrictedGoldArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.ActiveTrustedStandardUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedGoldNonPaymentArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void TrustedUser_Allowed_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.ActiveTrustedStandardUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedPaymentArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void TrustedUser_Denied_In_AdministrativeArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.ActiveTrustedStandardUser,
                IdentityRequirements = IdentityRequirementsGenerator.AdministrativeArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDenied, context.SecurityResponseCode);
            Assert.AreEqual(false, context.ExecutionStateValid);
        }



        [Test]
        public void AdminUser_Allowed_In_Public_Area()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.AdminUser,
                IdentityRequirements = IdentityRequirementsGenerator.PublicArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void AdminUser_Allowed_In_RestrictedStandardArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.AdminUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedStandardNonPaymentArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void AdminUser_Allowed_In_RestrictedGoldArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.AdminUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedGoldNonPaymentArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void AdminUser_Allowed_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.AdminUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedPaymentArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void AdminUser_Allowed_In_AdministrativeArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.AdminUser,
                IdentityRequirements = IdentityRequirementsGenerator.AdministrativeArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }



        [Test]
        public void SupremeUser_Allowed_In_Public_Area()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.SupremeUser,
                IdentityRequirements = IdentityRequirementsGenerator.PublicArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void SupremeUser_Allowed_In_RestrictedStandardArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.SupremeUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedStandardNonPaymentArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void SupremeUser_Allowed_In_RestrictedGoldArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.SupremeUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedGoldNonPaymentArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void SupremeUser_Allowed_In_RestrictedPaymentArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.SupremeUser,
                IdentityRequirements = IdentityRequirementsGenerator.RestrictedPaymentArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void SupremeUser_Allowed_In_AdministrativeArea()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.SupremeUser,
                IdentityRequirements = IdentityRequirementsGenerator.AdministrativeArea,
            };

            var step = new RoleAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }
    }
}