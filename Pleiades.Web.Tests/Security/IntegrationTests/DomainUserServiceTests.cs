using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Gallio.Framework;
using MbUnit.Framework;
using PagedList;
using Pleiades.Utilities.General;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Concrete;

namespace Pleiades.Web.Tests.Security.IntegrationTests
{
    /// <summary>
    /// Basic Domain User Retrieval Integration Tests
    /// </summary>
    [TestFixture]
    [Ignore]
    public class DomainUserServiceTests
    {
        [SetUp]
        public void ClearDataAndCreateNewAdmin()
        {
            UserTestHelpers.ResetTestUser();
        }

        [Test]
        public void RetrieveUserByEmailAddress()
        {
            var service = new DomainUserService();
            var user = service.RetrieveUserByEmail(UserTestHelpers.AdminUsers[0].Email);
            Assert.IsNotNull(user);
        }

        [Test]
        public void RetrieveUserByMembershipUserName()
        {
            var service = new DomainUserService();
            var user = service.RetrieveUserByEmail(UserTestHelpers.AdminUsers[0].Email);
            Assert.IsNotNull(user);
             
            var user2 = service.RetrieveUserByMembershipUserName(user.MembershipUser.UserName);
            Assert.IsNotNull(user2);
        }

        [Test]
        public void RetrieveUserByDomainUserKey()
        {
            var service = new DomainUserService();
            var user = service.RetrieveUserByEmail(UserTestHelpers.AdminUsers[0].Email);
            Assert.IsNotNull(user);

            var user2 = service.RetrieveUserByDomainUserId(user.DomainUserId);
            Assert.IsNotNull(user2);
        }

        [Test]
        public void RetreiveAllUsers()
        {
            var service = new DomainUserService();
            var page1 = service.RetreiveAll(1, 99, new List<UserRole>() { UserRole.Admin });
            Assert.AreEqual(5, page1.Count());
        }

        [Test]
        public void RetreiveUsersByEmail()
        {
            var email = UserTestHelpers.AdminUsers[2].Email;
            var service = new DomainUserService();
            var page1 = service.RetreiveByLikeEmail(email, 1, 99, new List<UserRole>() { UserRole.Admin });
            Assert.AreEqual(1, page1.Count());
            Assert.IsNotNull(page1.FirstOrDefault(x => x.Email == email));
        }

        [Test]
        [ExpectedException(typeof(Exception), "Maximum number of Admin Users is 5")]
        public void MaximumNumberOfAdmins()
        {
            var service = new DomainUserService();
            var admins = service.RetreiveAll(1, 999, new List<UserRole>() { UserRole.Admin });
            int numexisting = admins.Count;

            Assert.AreEqual(5, numexisting);                
            MembershipCreateStatus status;
            service.Create(UserTestHelpers.AnotherAdmin, out status);
        }

        [TearDown]
        public void Teardown()
        {
            UserTestHelpers.CleanupTestUser();
        }
    }
}
