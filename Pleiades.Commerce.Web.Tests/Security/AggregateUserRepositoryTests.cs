using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Commerce.IntegrationTests.Security
{
    [TestFixture]
    public class AggregateUserRepositoryTests
    {
        [Test]
        public void TestThatRepo_And_Verify()
        {
            // TODO...
        }

        //[Test]
        //public void RetrieveUserByEmailAddress()
        //{
        //    var service = new DomainUserService();
        //    var user = service.RetrieveUserByEmail(UserTestHelpers.AdminUsers[0].Email);
        //    Assert.IsNotNull(user);
        //}

        //[Test]
        //public void RetrieveUserByMembershipUserName()
        //{
        //    var service = new DomainUserService();
        //    var user = service.RetrieveUserByEmail(UserTestHelpers.AdminUsers[0].Email);
        //    Assert.IsNotNull(user);

        //    var user2 = service.RetrieveUserByMembershipUserName(user.MembershipUser.UserName);
        //    Assert.IsNotNull(user2);
        //}
    }
}
