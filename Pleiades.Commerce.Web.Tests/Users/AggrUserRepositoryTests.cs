using System;
using System.Collections.Generic;
using Pleiades.Commerce.Domain.Model.Users;
using Pleiades.Commerce.Persist;
using Pleiades.Commerce.Persist.Users;
using Pleiades.Framework.Data;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.MembershipProvider.Model;
using NUnit.Framework;

namespace Pleiades.Commerce.Web.IntegrationTests.Users
{
    [TestFixture]
    public class AggrUserRepositoryTests
    {
        [TestFixtureSetUp]
        public void BuildDatabase()
        {
            Console.WriteLine("Creating Database for Integration Testing of AggregateUserRepository");

            var context = new PleiadesContext(Constants.DatabaseConnString);
            if (context.Database.Exists())
            {
                context.Database.Delete();
            }

            context.Database.Create();
        }

        [Test]
        public void Verify_RetrieveUserByMembershipUserName()
        {
            var context = new PleiadesContext(Constants.DatabaseConnString);
            var repository = new AggregateUserRepository(context);

            var user1 = new AggregateUser
            {
                IdentityUser = new IdentityUser
                {
                    FirstName = "John",
                    LastName = "Gerber",
                },
                MembershipUser = new MembershipUser
                {
                    Email = "john@gerber.com",
                    UserName = "12345678",
                }
            };

            var user2 = new AggregateUser
            {
                IdentityUser = new IdentityUser
                {
                    FirstName = "Anne",
                    LastName = "Holtz",
                },
                MembershipUser = new MembershipUser
                {
                    Email = "anne@holtz.com",
                    UserName = "55555555",
                }
            };

            repository.Add(user1);
            repository.Add(user2);
            repository.SaveChanges();

            var testUser = repository.RetrieveUserByMembershipUserName("55555555");

            Assert.IsNotNull(testUser);
            Assert.AreEqual("Anne", testUser.IdentityUser.FirstName);
            Assert.AreEqual("Holtz", testUser.IdentityUser.LastName);
            Assert.AreEqual("anne@holtz.com", testUser.MembershipUser.Email);
        }
    }
}
