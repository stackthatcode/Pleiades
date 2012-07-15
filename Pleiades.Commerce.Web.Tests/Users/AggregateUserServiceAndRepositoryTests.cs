using System;
using System.Web.Security;
using System.Collections.Generic;
using System.Linq;
using Pleiades.Commerce.Domain.Concrete;
using Pleiades.Commerce.Domain.Interface;
using Pleiades.Commerce.Domain.Model.Users;
using Pleiades.Commerce.Persist;
using Pleiades.Commerce.Persist.Users;
using Pleiades.Framework.Data;
using Pleiades.Framework.Identity.Concrete;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.MembershipProvider.Concrete;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Framework.MembershipProvider.Model;
using Pleiades.Framework.MembershipProvider.Providers;
using Pleiades.Framework.Utility;
using NUnit.Framework;


namespace Pleiades.Commerce.Web.IntegrationTests.Users
{
    [TestFixture]
    public class AggregateUserServiceAndRepositoryTests
    {
        IAggregateUserService Service { get; set; }
        IAggregateUserRepository AggregateUserRepository { get; set; }
        
        [TestFixtureSetUp]
        public void Setup()
        {
            var context = new PleiadesContext(Constants.DatabaseConnString);
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
            var context = new PleiadesContext(Constants.DatabaseConnString);

            var identityuser1 = new CreateOrModifyIdentityUserRequest
                {
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
            var testUser = this.AggregateUserRepository.RetrieveUserByMembershipUserName(membershipUserName);

            Assert.IsNotNull(testUser);
            Assert.AreEqual("Anne", testUser.IdentityUser.FirstName);
            Assert.AreEqual("Holtz", testUser.IdentityUser.LastName);
            Assert.AreEqual("anne@holtz.com", "anne@holtz.com");
        }
    }
}
