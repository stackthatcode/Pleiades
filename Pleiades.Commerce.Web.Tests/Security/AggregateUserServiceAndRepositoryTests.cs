using System;
using System.Linq;
using System.Web.Security;
using NUnit.Framework;
using Pleiades.Data;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Providers;
using Pleiades.Utility;
using Commerce.Persist;
using Commerce.Persist.Security;

namespace Commerce.IntegrationTests.Security
{
    [TestFixture]
    public class AggregateUserServiceAndRepositoryTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            // Empty the test data
            DatabasePriming.CleanOutTheDatabase();

            // When we create a new MembershipService, the Shim will inject the DbContext
            DatabasePriming.InitializeMembership(); 
        }

        public AggregateUserService CreateAggregateUserService()
        {            
            // Create the Aggregate User Service with a separate Database Context
            var dbContext2 = new PleiadesContext();
            var membershipService = new MembershipService();
            var aggregateUserRepository = new AggregateUserRepository(dbContext2);

            return new AggregateUserService(membershipService, aggregateUserRepository);
        }

        [Test]
        public void Verify_Create_And_RetrieveByMembershipUserName()
        {
            // Arrange
            var context = new PleiadesContext();

            var identityuser1 = new CreateOrModifyIdentityRequest
                {
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

            var service = this.CreateAggregateUserService();
            service.Create(membershipuser1, identityuser1, out outstatus1);
            service.Create(membershipuser2, identityuser2, out outstatus2);

            // Assert
            var membershipService = new MembershipService();
            var membershipUserName = membershipService.GetUserNameByEmail("anne@holtz.com");

            var aggregateUserRepository = new AggregateUserRepository(context);
            var testUser = aggregateUserRepository.RetrieveByMembershipUserName(membershipUserName);

            Assert.IsNotNull(testUser);
            Assert.AreEqual("Anne", testUser.IdentityProfile.FirstName);
            Assert.AreEqual("Holtz", testUser.IdentityProfile.LastName);
            Assert.AreEqual("anne@holtz.com", "anne@holtz.com");
        }

        [Test]
        public void Verify_UpdateIdentity_And_RetreiveByRetrieveById()
        {
            // Arrange
            var context = new PleiadesContext();            
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
            var service = this.CreateAggregateUserService();
            var result = service.Create(membershipuser2, identityuser2, out outstatus2);

            // Act
            var modificationRequeset = new CreateOrModifyIdentityRequest
            {
                 AccountLevel = AccountLevel.Standard,
                 AccountStatus = AccountStatus.PaymentRequired,
                 UserRole = UserRole.Trusted,
                 FirstName = "diane",
                 LastName = "moon"
            };

            var aggregateUserRepository = new AggregateUserRepository(context);
            aggregateUserRepository.UpdateIdentity(result.ID, modificationRequeset);
            var updatedResult = aggregateUserRepository.RetrieveById(result.ID);

            // Assert
            Assert.AreEqual(AccountLevel.Standard, updatedResult.IdentityProfile.AccountLevel);
            Assert.AreEqual(AccountStatus.PaymentRequired, updatedResult.IdentityProfile.AccountStatus);
            Assert.AreEqual(UserRole.Trusted, updatedResult.IdentityProfile.UserRole);
            Assert.AreEqual("diane", updatedResult.IdentityProfile.FirstName);
            Assert.AreEqual("moon", updatedResult.IdentityProfile.LastName);
        }

        [Test]
        public void Verify_RetrieveByUserRoles()
        {
            // Arrange
        }

        //[Test]
        //public void Verify_RetrieveUserById()
        //{
        //    // Arrange       
        //    var repository = MockRepository.GenerateMock<IIdentityRepository>();
        //    var identityService = new IdentityUserService(repository);
        //    var testUser = new IdentityProfile() { ID = 123 };
        //    repository.Expect(x => x.RetrieveUserById(123)).Return(testUser);

        //    // Act
        //    var user = identityService.RetrieveUserById(123);

        //    // Assert
        //    repository.VerifyAllExpectations();
        //    Assert.AreEqual(123, user.ID);
        //}

        //[Test]
        //public void RetrieveUserByEmailAddress()
        //{
        //    var service = new DomainUserService();
        //    var user = service.RetrieveUserByEmail(UserTestHelpers.AdminUsers[0].Email);
        //    Assert.IsNotNull(user);
        //}


        // TODO LATER: Verify_RetreiveByMembershipUserNamesAndUserRoles()
    }
}