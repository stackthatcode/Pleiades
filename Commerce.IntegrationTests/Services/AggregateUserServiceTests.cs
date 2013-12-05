using System.Collections.Generic;
using System.Linq;
using Autofac;
using NUnit.Framework;
using Pleiades.App.Data;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.IntegrationTests.Services
{
    [TestFixture]
    public class AggregateUserServiceTests : FixtureBase
    {
        [Test]
        public void Create_And_RetrieveByMembershipUserName()
        {
            using (var lifetime = TestContainer.LifetimeScope())
            {
                // Arrange
                var identityuser1 = new IdentityProfileChange
                    {
                        AccountStatus = AccountStatus.Active,
                        UserRole = UserRole.Trusted,
                        AccountLevel = AccountLevel.Standard,
                        FirstName = "John",
                        LastName = "Gerber",
                    };
                var membershipuser1 = new PfCreateNewMembershipUserRequest
                    {
                        Email = "john@gerber.com",
                        Password = "1234567890",
                    };

                var identityuser2 = new IdentityProfileChange
                    {
                        AccountStatus = AccountStatus.Active,
                        UserRole = UserRole.Trusted,
                        AccountLevel = AccountLevel.Gold,
                        FirstName = "Anne",
                        LastName = "Holtz",
                    };
                var membershipuser2 = new PfCreateNewMembershipUserRequest
                    {
                        Email = "anne@holtz.com",
                        Password = "1234567890",
                    };

                // Act
                string outstatus1, outstatus2;

                var service = lifetime.Resolve<IAggregateUserService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();
                service.Create(membershipuser1, identityuser1, out outstatus1);
                service.Create(membershipuser2, identityuser2, out outstatus2);
                unitOfWork.SaveChanges();
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                // Assert
                var membershipService = lifetime.Resolve<IPfMembershipService>();
                var membershipUser = membershipService.GetUserByEmail("anne@holtz.com");

                var aggregateUserRepository = lifetime.Resolve<IReadOnlyAggregateUserRepository>();
                var testUser = aggregateUserRepository.RetrieveByMembershipUserName(membershipUser.UserName);

                Assert.IsNotNull(testUser);
                Assert.AreEqual("Anne", testUser.IdentityProfile.FirstName);
                Assert.AreEqual("Holtz", testUser.IdentityProfile.LastName);
                Assert.AreEqual("anne@holtz.com", "anne@holtz.com");
            }
        }

        [Test]
        public void UpdateIdentity_And_RetreiveByRetrieveById()
        {
            AggregateUser result;
            using (var lifetime = TestContainer.LifetimeScope())
            {
                // Arrange
                var identityuser2 = new IdentityProfileChange
                    {
                        AccountLevel = AccountLevel.Gold,
                        AccountStatus = AccountStatus.Active,
                        UserRole = UserRole.Admin,
                        FirstName = "Diana",
                        LastName = "Moon",
                    };
                var membershipuser2 = new PfCreateNewMembershipUserRequest
                    {
                        Email = "dianemoon@city17.gov",
                        Password = "1234567890",
                    };

                string outstatus2;
                var service = lifetime.Resolve<IAggregateUserService>();
                result = service.Create(membershipuser2, identityuser2, out outstatus2);
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();
                unitOfWork.SaveChanges();
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var service = lifetime.Resolve<IAggregateUserService>();
                var modificationRequeset = new IdentityProfileChange
                    {
                        AccountLevel = AccountLevel.Standard,
                        AccountStatus = AccountStatus.PaymentRequired,
                        UserRole = UserRole.Trusted,
                        FirstName = "diane",
                        LastName = "moon"
                    };

                service.UpdateIdentity(result.ID, modificationRequeset);
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();
                unitOfWork.SaveChanges();
            }

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var aggregateUserRepository = lifetime.Resolve<IReadOnlyAggregateUserRepository>();
                var updatedResult = aggregateUserRepository.RetrieveById(result.ID);

                // Assert
                Assert.AreEqual(AccountLevel.Standard, updatedResult.IdentityProfile.AccountLevel);
                Assert.AreEqual(AccountStatus.PaymentRequired, updatedResult.IdentityProfile.AccountStatus);
                Assert.AreEqual(UserRole.Trusted, updatedResult.IdentityProfile.UserRole);
                Assert.AreEqual("diane", updatedResult.IdentityProfile.FirstName);
                Assert.AreEqual("moon", updatedResult.IdentityProfile.LastName);
            }
        }

        [Test]
        public void RetrieveByUserRoles()
        {
            using (var lifetime = TestContainer.LifetimeScope())
            {
                // Arrange
                var identityuser2 = new IdentityProfileChange
                    {
                        AccountLevel = AccountLevel.Gold,
                        AccountStatus = AccountStatus.Active,
                        UserRole = UserRole.Admin,
                        FirstName = "User",
                        LastName = "X",
                    };
                var membershipuser2 = new PfCreateNewMembershipUserRequest
                    {
                        Email = "userx@city17.gov",
                        Password = "1234567890",
                    };

                // Arrange
                var identityuser1 = new IdentityProfileChange
                    {
                        AccountLevel = AccountLevel.Gold,
                        AccountStatus = AccountStatus.Active,
                        UserRole = UserRole.Trusted,
                        FirstName = "User",
                        LastName = "Y",
                    };
                var membershipuser1 = new PfCreateNewMembershipUserRequest
                    {
                        Email = "userYYYY@city17.gov",
                        Password = "1234567890",
                    };

                string outstatus;
                var service = lifetime.Resolve<IAggregateUserService>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();
                var result1 = service.Create(membershipuser1, identityuser1, out outstatus);
                var result2 = service.Create(membershipuser2, identityuser2, out outstatus);
                unitOfWork.SaveChanges();
            }

            // Act
            using (var lifetime = TestContainer.LifetimeScope())
            {
                var repository = lifetime.Resolve<IReadOnlyAggregateUserRepository>();
                var adminsOnly = repository.Retreive(new List<UserRole>() {UserRole.Admin});
                var trustedOnly = repository.Retreive(new List<UserRole>() {UserRole.Trusted});
                var bothUserTypes = repository.Retreive(new List<UserRole>() {UserRole.Admin, UserRole.Trusted});

                // Assert
                Assert.IsTrue(adminsOnly.All(x => x.IdentityProfile.UserRole == UserRole.Admin));
                Assert.IsTrue(trustedOnly.All(x => x.IdentityProfile.UserRole == UserRole.Trusted));
                Assert.IsTrue(bothUserTypes.Any(x => x.IdentityProfile.UserRole == UserRole.Trusted));
                Assert.IsTrue(bothUserTypes.Any(x => x.IdentityProfile.UserRole == UserRole.Admin));
            }
        }
    }
}