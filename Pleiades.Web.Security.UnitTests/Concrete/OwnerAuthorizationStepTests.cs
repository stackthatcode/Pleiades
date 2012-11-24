using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Model;

namespace Pleiades.UnitTests.Web.Security.Aggregate.Step
{
    [TestFixture]
    public class OwnerAuthorizationStepTests
    {
        // *** TODO - add test for Authorize(ownerId) *** //

        [Test]
        public void Null_ResourceOwnerIdentityUserId_Allows_Access_To_Anybody()
        {
            // Arrange
            var currentUser = new AggregateUser { IdentityProfile = new IdentityProfile { ID = 333, } };
            var resourceOwner = (AggregateUser)null;
            var service = new OwnerAuthorizationService(null, null);

            // Act
            var result = service.Authorize(currentUser, resourceOwner);

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, result);
        }

        [Test]
        public void ResourceOwner_Can_Access_Own_Resources()
        {
            // Arrange
            var currentUser = new AggregateUser { IdentityProfile = new IdentityProfile { ID = 333, } };
            var resourceOwner = new AggregateUser { IdentityProfile = new IdentityProfile { ID = 333, } };
            var service = new OwnerAuthorizationService(null, null);

            // Act
            var result = service.Authorize(currentUser, resourceOwner);

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, result);
        }

        [Test]
        public void ResourceNonOwner_Denied_Resources()
        {
            // Arrange
            var currentUser = new AggregateUser { IdentityProfile = new IdentityProfile { ID = 444, } };
            var resourceOwner = new AggregateUser { IdentityProfile = new IdentityProfile { ID = 333, } };
            var service = new OwnerAuthorizationService(null, null);

            // Act
            var result = service.Authorize(currentUser, resourceOwner);

            // Assert
            Assert.AreEqual(SecurityCode.AccessDenied, result);
        }

        [Test]
        public void Admin_Cannot_Operate_On_Supreme()
        {
            // Arrange
            var currentUser = new AggregateUser { IdentityProfile = StubUserGenerator.AdminUser };
            var resourceOwner = new AggregateUser { IdentityProfile = StubUserGenerator.SupremeUser };
            var service = new OwnerAuthorizationService(null, null);

            // Act
            var result = service.Authorize(currentUser, resourceOwner);

            // Assert
            Assert.AreEqual(SecurityCode.AccessDenied, result);
        }

        [Test]
        public void Admin_Can_Operate_On_NonAdmin()
        {
            // Arrange
            var currentUser = new AggregateUser { IdentityProfile = StubUserGenerator.AdminUser };
            var resourceOwner = new AggregateUser { IdentityProfile = StubUserGenerator.ActiveTrustedGoldUser };
            var service = new OwnerAuthorizationService(null, null);

            // Act
            var result = service.Authorize(currentUser, resourceOwner);

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, result);
        }

        [Test]
        public void Supreme_Can_Operate_On_Supreme()
        {
            // Arrange
            var currentUser = new AggregateUser { IdentityProfile = StubUserGenerator.SupremeUser };
            var resourceOwner = new AggregateUser { IdentityProfile = StubUserGenerator.SupremeUser };

            var service = new OwnerAuthorizationService(null, null);

            // Act
            var result = service.Authorize(currentUser, resourceOwner);

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, result);
        }
    }
}