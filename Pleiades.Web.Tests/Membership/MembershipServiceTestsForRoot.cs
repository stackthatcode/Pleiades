using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Gallio.Framework;
using MbUnit.Framework;
using Pleiades.Framework.Web.Security.Model;
using Pleiades.Framework.Web.Security.Concrete;

namespace Pleiades.Web.Tests.Security.IntegrationTests
{
    [TestFixture]
    [Ignore]
    public class MembershipServiceTestsForRoot
    {
        public DomainUser Root
        {
            get
            {
                return new DomainUserService().RetrieveUserByEmail("aleksjones@gmail.com");
            }
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void CantResetPasswordForRoot()
        {
            // Reset the Password
            var membershipUserService = new MembershipService();
            var resetPwd = membershipUserService.ResetPassword(this.Root);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void CantDisapproveRoot()
        {
            // Disapprove User
            var membershipService = new MembershipService();
            membershipService.SetUserApproval(this.Root, false);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void CantChangeUserEmailAddressForRoot()
        {            
            // Change the password
            var membershipUserService = new MembershipService();
            membershipUserService.ChangeEmailAddress(this.Root, "bob@bob.com");
        }
    }
}
