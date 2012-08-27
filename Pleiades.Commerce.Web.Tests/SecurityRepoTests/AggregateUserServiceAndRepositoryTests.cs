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


namespace CommerceIntegrationTests.AggregateUser
{
    [TestFixture]
    public class AggregateUserServiceAndRepositoryTests
    {
        IAggregateUserService Service { get; set; }
        IAggregateUserRepository AggregateUserRepository { get; set; }
        
        [TestFixtureSetUp]
        public void Setup()
        {
            var context = new PleiadesContext();
            if (!context.Database.Exists())
            {
                // Build Database
                Console.WriteLine("Creating Database for Integration Testing of AggregateUserRepository");
                context.Database.Create();
            }

            // Prepare Repository Factory
            PfMembershipRepositoryShim.RepositoryFactory =
                () =>
                {
                    var _dbContext = context;
                    var _repository = new MembershipRepository(context);
                    _repository.ApplicationName = System.Web.Security.Membership.ApplicationName;
                    return _repository;
                };
            var membershipService = new MembershipService();

            // Initialize Identity
            var identityRepository = new IdentityUserRepository(context);
            var identityUserService = new IdentityUserService(identityRepository);

            // Initialize Aggregate User Service
            this.AggregateUserRepository = new AggregateUserRepository(context);
            this.Service = new AggregateUserService(membershipService, identityUserService, this.AggregateUserRepository);

            // Clean it out
            identityRepository.GetAll().ForEach(x => identityRepository.Delete(x));

            var membershipRepository = PfMembershipRepositoryShim.RepositoryFactory();
            membershipRepository.GetAll().ForEach(x => membershipRepository.Delete(x));
            this.AggregateUserRepository.GetAll().ForEach(x => this.AggregateUserRepository.Delete(x));
            context.SaveChanges();
        }

        [Test]
        public void Verify_CanCreate_And_RetrieveUser_ByMembershipUserName()
        {
            var context = new PleiadesContext();

            var identityuser1 = new CreateOrModifyIdentityUserRequest
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

            var identityuser2 = new CreateOrModifyIdentityUserRequest
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

            PleiadesMembershipCreateStatus outstatus1, outstatus2;
            this.Service.Create(membershipuser1, identityuser1, out outstatus1);
            this.Service.Create(membershipuser2, identityuser2, out outstatus2);

            var membershipService = new MembershipService();
            var membershipUserName = membershipService.GetUserNameByEmail("anne@holtz.com");
            var testUser = this.AggregateUserRepository.RetrieveByMembershipUserName(membershipUserName);

            Assert.IsNotNull(testUser);
            Assert.AreEqual("Anne", testUser.IdentityUser.FirstName);
            Assert.AreEqual("Holtz", testUser.IdentityUser.LastName);
            Assert.AreEqual("anne@holtz.com", "anne@holtz.com");
        }
    }
}