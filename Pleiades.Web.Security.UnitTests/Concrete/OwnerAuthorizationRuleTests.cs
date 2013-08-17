using NUnit.Framework;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Rules;
using Pleiades.Web.Security.UnitTests.Rules;

namespace Pleiades.Web.Security.UnitTests.Concrete
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
            
            // Act
            var result = OwnerAuthorizeRules.Authorize(currentUser, resourceOwner);

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, result);
        }

        [Test]
        public void ResourceOwner_Can_Access_Own_Resources()
        {
            // Arrange
            var currentUser = new AggregateUser { IdentityProfile = new IdentityProfile { ID = 333, } };
            var resourceOwner = new AggregateUser { IdentityProfile = new IdentityProfile { ID = 333, } };

            // Act
            var result = OwnerAuthorizeRules.Authorize(currentUser, resourceOwner);

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, result);
        }

        [Test]
        public void ResourceNonOwner_Denied_Resources()
        {
            // Arrange
            var currentUser = new AggregateUser { IdentityProfile = new IdentityProfile { ID = 444, } };
            var resourceOwner = new AggregateUser { IdentityProfile = new IdentityProfile { ID = 333, } };
            
            // Act
            var result = OwnerAuthorizeRules.Authorize(currentUser, resourceOwner);

            // Assert
            Assert.AreEqual(SecurityCode.AccessDenied, result);
        }

        [Test]
        public void Admin_Cannot_Operate_On_Supreme()
        {
            // Arrange
            var currentUser = new AggregateUser { IdentityProfile = StubUserGenerator.AdminUser };
            var resourceOwner = new AggregateUser { IdentityProfile = StubUserGenerator.SupremeUser };
            
            // Act
            var result = OwnerAuthorizeRules.Authorize(currentUser, resourceOwner);

            // Assert
            Assert.AreEqual(SecurityCode.AccessDenied, result);
        }

        [Test]
        public void Admin_Can_Operate_On_NonAdmin()
        {
            // Arrange
            var currentUser = new AggregateUser { IdentityProfile = StubUserGenerator.AdminUser };
            var resourceOwner = new AggregateUser { IdentityProfile = StubUserGenerator.ActiveTrustedGoldUser };
            
            // Act
            var result = OwnerAuthorizeRules.Authorize(currentUser, resourceOwner);

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, result);
        }

        [Test]
        public void Supreme_Can_Operate_On_Supreme()
        {
            // Arrange
            var currentUser = new AggregateUser { IdentityProfile = StubUserGenerator.SupremeUser };
            var resourceOwner = new AggregateUser { IdentityProfile = StubUserGenerator.SupremeUser };

            // Act
            var result = OwnerAuthorizeRules.Authorize(currentUser, resourceOwner);

            // Assert
            Assert.AreEqual(SecurityCode.Allowed, result);
        }
    }
}