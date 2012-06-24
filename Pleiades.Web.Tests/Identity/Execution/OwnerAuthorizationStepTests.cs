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
    public class OwnerAuthorizationStepTests
    {
        [Test]
        public void Null_ResourceOwnerIdentityUserId_Allows_Access_To_Anybody()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = new IdentityUser
                {
                    ID = 333,
                },
                IdentityRequirements = new IdentityRequirements()
                {
                    ResourceOwnerIdentityUserId = null,
                }
            };

            var step = new SimpleOwnerAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }


        [Test]
        public void ResourceOwner_Can_Access_Own_Resources()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = new IdentityUser
                {
                    ID = 333,
                },
                IdentityRequirements = new IdentityRequirements() 
                {
                    ResourceOwnerIdentityUserId = 333,
                }
            };

            var step = new SimpleOwnerAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

        [Test]
        public void ResourceNonOwner_Denied_Resources()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = new IdentityUser
                {
                    ID = 444,
                },
                IdentityRequirements = new IdentityRequirements()
                {
                    ResourceOwnerIdentityUserId = 333,
                }
            };

            var step = new SimpleOwnerAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.AccessDenied, context.SecurityResponseCode);
            Assert.AreEqual(false, context.ExecutionStateValid);
        }

        [Test]
        public void Admin_Can_Access_Anything()
        {
            // Arrange
            var context = new StubSecurityContext()
            {
                IdentityUser = StubUserGenerator.AdminUser,
                IdentityRequirements = new IdentityRequirements()
                {
                    ResourceOwnerIdentityUserId = 333,
                }
            };

            var step = new SimpleOwnerAuthorizationStep<StubSecurityContext>();

            // Act
            step.Execute(context);

            // Assert
            Assert.AreEqual(SecurityResponseCode.Allowed, context.SecurityResponseCode);
            Assert.AreEqual(true, context.ExecutionStateValid);
        }

    }
}