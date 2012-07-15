using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

using Pleiades.Framework.Helpers;
using Pleiades.Framework.Web.Security.Model;
using Pleiades.Framework.Web.Security.Concrete;

namespace Pleiades.Web.Tests.Security.IntegrationTests
{
    /// <summary>
    /// Basic Identity User Retrieval Integration Tests
    /// </summary>
    [TestFixture]
    [Ignore]
    public class DomainUserServiceTests
    {

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
    }
}
