using System.Collections.Generic;
using System.Linq;
using Autofac;
using NUnit.Framework;
using Pleiades.Data;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.IntegrationTests.Security
{
    [TestFixture]
    public class AggregateUserServiceTests : FixtureBase
    {
        // TODO LATER: Verify_RetreiveByMembershipUserNamesAndUserRoles()

        [TestFixtureSetUp]
        public void Setup()
        {
            // Empty the test data
            FixtureBase.CleanOutUserData();
        }

        [Test]
        public void Create_And_RetrieveByMembershipUserName()
        {
            var lifetime = TestContainer.LifetimeScope();

            // Arrange
            var identityuser1 = new CreateOrModifyIdentityRequest
                {
                    AccountStatus = AccountStatus.Active,
                    UserRole = UserRole.Trusted,
                    AccountLevel = AccountLevel.Standard,
                    FirstName = "John",
                    LastName = "Gerber",
                };
            var membershipuser1 = new CreateNewMembershipUserRequest
                {
                    Email = "john@gerber.com",
                    Password = "1234567890",
                };

            var identityuser2 = new CreateOrModifyIdentityRequest
                {
                    AccountStatus = AccountStatus.Active,
                    UserRole = UserRole.Trusted,
                    AccountLevel = AccountLevel.Gold,
                    FirstName = "Anne",
                    LastName = "Holtz",
                };
            var membershipuser2 = new CreateNewMembershipUserRequest
                {
                    Email = "anne@holtz.com",
                    Password = "1234567890",
                };

            // Act
            PleiadesMembershipCreateStatus outstatus1, outstatus2;

            var service = lifetime.Resolve<IAggregateUserService>();
            service.Create(membershipuser1, identityuser1, out outstatus1);
            service.Create(membershipuser2, identityuser2, out outstatus2);

            // Assert
            var membershipService = lifetime.Resolve<IMembershipService>();
            var membershipUserName = membershipService.GetUserNameByEmail("anne@holtz.com");

            var aggregateUserRepository = lifetime.Resolve<IAggregateUserRepository>();
            var testUser = aggregateUserRepository.RetrieveByMembershipUserName(membershipUserName);

            Assert.IsNotNull(testUser);
            Assert.AreEqual("Anne", testUser.IdentityProfile.FirstName);
            Assert.AreEqual("Holtz", testUser.IdentityProfile.LastName);
            Assert.AreEqual("anne@holtz.com", "anne@holtz.com");
        }

        [Test]
        public void UpdateIdentity_And_RetreiveByRetrieveById()
        {
            var lifetime = TestContainer.LifetimeScope();

            // Arrange
            var identityuser2 = new CreateOrModifyIdentityRequest
            {
                AccountLevel = AccountLevel.Gold,
                AccountStatus = AccountStatus.Active,
                UserRole = UserRole.Admin,
                FirstName = "Diana",
                LastName = "Moon",
            };
            var membershipuser2 = new CreateNewMembershipUserRequest
            {
                Email = "dianemoon@city17.gov",
                Password = "1234567890",
            };

            PleiadesMembershipCreateStatus outstatus2;
            var service = lifetime.Resolve<IAggregateUserService>();
            var result = service.Create(membershipuser2, identityuser2, out outstatus2);

            // Act
            var modificationRequeset = new CreateOrModifyIdentityRequest
            {
                Id = result.ID,
                AccountLevel = AccountLevel.Standard,
                AccountStatus = AccountStatus.PaymentRequired,
                UserRole = UserRole.Trusted,
                FirstName = "diane",
                LastName = "moon"
            };

            var aggregateUserRepository = lifetime.Resolve<IAggregateUserRepository>();
            aggregateUserRepository.UpdateIdentity(modificationRequeset);

            var unitOfWork = lifetime.Resolve<IUnitOfWork>();
            unitOfWork.SaveChanges();

            var updatedResult = aggregateUserRepository.RetrieveById(result.ID);

            // Assert
            Assert.AreEqual(AccountLevel.Standard, updatedResult.IdentityProfile.AccountLevel);
            Assert.AreEqual(AccountStatus.PaymentRequired, updatedResult.IdentityProfile.AccountStatus);
            Assert.AreEqual(UserRole.Trusted, updatedResult.IdentityProfile.UserRole);
            Assert.AreEqual("diane", updatedResult.IdentityProfile.FirstName);
            Assert.AreEqual("moon", updatedResult.IdentityProfile.LastName);
        }

        [Test]
        public void RetrieveByUserRoles()
        {
            var lifetime = TestContainer.LifetimeScope();

            // Arrange
            var identityuser2 = new CreateOrModifyIdentityRequest
            {
                AccountLevel = AccountLevel.Gold,
                AccountStatus = AccountStatus.Active,
                UserRole = UserRole.Admin,
                FirstName = "User",
                LastName = "X",
            };
            var membershipuser2 = new CreateNewMembershipUserRequest
            {
                Email = "userx@city17.gov",
                Password = "1234567890",
            };

            // Arrange
            var identityuser1 = new CreateOrModifyIdentityRequest
            {
                AccountLevel = AccountLevel.Gold,
                AccountStatus = AccountStatus.Active,
                UserRole = UserRole.Trusted,
                FirstName = "User",
                LastName = "Y",
            };
            var membershipuser1 = new CreateNewMembershipUserRequest
            {
                Email = "userYYYY@city17.gov",
                Password = "1234567890",
            };

            PleiadesMembershipCreateStatus outstatus;
            var service = lifetime.Resolve<IAggregateUserService>();
            var result1 = service.Create(membershipuser1, identityuser1, out outstatus);
            var result2 = service.Create(membershipuser2, identityuser2, out outstatus);

            // Act
            var repository = lifetime.Resolve<IAggregateUserRepository>();
            var adminsOnly = repository.Retreive(new List<UserRole>() { UserRole.Admin });
            var trustedOnly = repository.Retreive(new List<UserRole>() { UserRole.Trusted });
            var bothUserTypes = repository.Retreive(new List<UserRole>() { UserRole.Admin, UserRole.Trusted });

            // Assert
            Assert.IsTrue(adminsOnly.All(x => x.IdentityProfile.UserRole == UserRole.Admin));
            Assert.IsTrue(trustedOnly.All(x => x.IdentityProfile.UserRole == UserRole.Trusted));
            Assert.IsTrue(bothUserTypes.Any(x => x.IdentityProfile.UserRole == UserRole.Trusted));
            Assert.IsTrue(bothUserTypes.Any(x => x.IdentityProfile.UserRole == UserRole.Admin));
        }
    }
}