using System;
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
    public class AggregateUserServiceTests
    {
        [TestFixtureSetUp]
        public void Setup()
        {
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
        public void Verify_CanCreate_And_RetrieveUser_ByMembershipUserName()
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
    }
}