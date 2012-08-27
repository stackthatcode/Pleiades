using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Execution;
using Pleiades.Security;
using Pleiades.Web.Security.Execution;
using Pleiades.Web.Security.Model;

namespace Pleiades.UnitTests.Identity.Execution
{
    [TestFixture]
    public class OwnerAuthorizationStepTests
    {
        [Test]
        public void Null_ResourceOwnerIdentityUserId_Allows_Access_To_Anybody()
        {
            // Arrange
            var context = new OwnerAuthorizationContext()
            {
                CurrentUserIdentity = new IdentityUser
                {
                    ID = 333,
                },
                OwnerIdentityId = null,
            };

            var step = new SimpleOwnerAuthorizationStep<OwnerAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }


        [Test]
        public void ResourceOwner_Can_Access_Own_Resources()
        {
            // Arrange
            var context = new OwnerAuthorizationContext()
            {
                CurrentUserIdentity = new IdentityUser
                {
                    ID = 333,
                },
                OwnerIdentityId = 333,
            };

            var step = new SimpleOwnerAuthorizationStep<OwnerAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

        [Test]
        public void ResourceNonOwner_Denied_Resources()
        {
            // Arrange
            var context = new OwnerAuthorizationContext()
            {
                CurrentUserIdentity = new IdentityUser
                {
                    ID = 444,
                },
                OwnerIdentityId = 333,
            };

            var step = new SimpleOwnerAuthorizationStep<OwnerAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDenied, context.SecurityResponseCode);
            Assert.AreEqual(false, context.IsExecutionStateValid);
        }

        [Test]
        public void Admin_Can_Access_Anything()
        {
            // Arrange
            var context = new OwnerAuthorizationContext()
            {
                CurrentUserIdentity = StubUserGenerator.AdminUser,
                OwnerIdentityId = 333,
            };

            var step = new SimpleOwnerAuthorizationStep<OwnerAuthorizationContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.IsExecutionStateValid);
        }

    }
}