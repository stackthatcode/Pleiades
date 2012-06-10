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
    public class MembershipServiceTests
    {
        [SetUp]
        public void TestSetup()
        {
            UserTestHelpers.ResetTestUser();
        }

        #region User Validation
        [Test]
        public void AuthenticationTest()
        {
            var service = new MembershipService();
            var result = 
                service.ValidateUserByEmailAddr(
                    UserTestHelpers.AdminUsers[0].Email, UserTestHelpers.AdminUsers[0].Password);
            Assert.IsNotNull(result);
        }

        [Test]
        public void FailedAuthenticationTest()
        {
            var service = new MembershipService();
            var result =
                service.ValidateUserByEmailAddr(
                    UserTestHelpers.AdminUsers[0].Email, "Garbage Password");
            Assert.IsNull(result);
        }

        [Test]
        public void AuthenticationDisableUserTest()
        {
            var service = new MembershipService();
            var result = service.ValidateUserByEmailAddr(
                UserTestHelpers.TrustedUsers[4].Email, UserTestHelpers.TrustedUsers[4].Password);
            Assert.IsNull(result);
        }

        [Test]
        public void AuthenticationDelinquenstUserTest()
        {
            var service = new MembershipService();
            var result = service.ValidateUserByEmailAddr(
                UserTestHelpers.TrustedUsers[3].Email, UserTestHelpers.TrustedUsers[3].Password);
            Assert.IsNull(result);
        }
        #endregion

        #region Password Reset/Change, Approval & Lockout Testing
        // NOTE: Should fail if the configuration requires question & answer
        [Test]
        public void PasswordServiceResetAndChangeTests()
        {
            // Get the user
            var user = UserTestHelpers.TrustedUsers[0];
            var userservice = new DomainUserService();
            var admin = userservice.RetrieveUserByEmail(user.Email);
            
            // Reset the Password
            var membershipUserService = new MembershipService();
            var resetPwd = membershipUserService.ResetPassword(admin);

            // Try authenticating with valid credentials
            var result = membershipUserService.ValidateUserByEmailAddr(user.Email, resetPwd);
            Assert.IsNotNull(result);

            // Try authenticating with invalid credentials
            var result2 = membershipUserService.ValidateUserByEmailAddr(user.Email, "fgghjkgaaewgw");
            Assert.IsNull(result2);
            
            // Now, use the reset password to change the Password
            membershipUserService.ChangePassword(admin, resetPwd, "password1");
            var result3 = membershipUserService.ValidateUserByEmailAddr(user.Email, "password1");
            Assert.IsNotNull(result3);
        }

        [Test]
        public void DisapproveAndApproveTesting()
        {
            // Get the user
            var userservice = new DomainUserService();
            var user = userservice.RetrieveUserByEmail(UserTestHelpers.TrustedUsers[0].Email);

            // Disapprove User
            var membershipService = new MembershipService();
            membershipService.SetUserApproval(user, false);

            // Try to Authenticate - should fail
            var result1 = membershipService.ValidateUserByEmailAddr(
                UserTestHelpers.TrustedUsers[0].Email, UserTestHelpers.TrustedUsers[0].Password);
            Assert.IsNull(result1);

            // Reapprove USer
            membershipService.SetUserApproval(user, true);

            // Try to Authenticate - should succeed
            var result2 = membershipService.ValidateUserByEmailAddr(
                UserTestHelpers.TrustedUsers[0].Email, UserTestHelpers.TrustedUsers[0].Password);
            Assert.IsNotNull(result2);
        }

        [Test]
        public void SimulateAccountLockoutAndUnlocking()
        {
            // Get the user
            var userservice = new DomainUserService();
            var user = userservice.RetrieveUserByEmail(UserTestHelpers.TrustedUsers[1].Email);

            // Disapprove User
            var membershipService = new MembershipService();

            // Get user
            for (int i = 0; i < 10; i++)
            {
                membershipService.ValidateUserByEmailAddr(
                    UserTestHelpers.TrustedUsers[1].Email, "wrongpassword");
            }
            
            // Should be locked out
            var result1 = membershipService.ValidateUserByEmailAddr(
                UserTestHelpers.TrustedUsers[1].Email, UserTestHelpers.TrustedUsers[1].Password);
            Assert.IsNull(result1);

            // Unlock the User
            membershipService.UnlockUser(user);
            
            // Should be able to authenticated
            var result2 = membershipService.ValidateUserByEmailAddr(
                UserTestHelpers.TrustedUsers[1].Email, UserTestHelpers.TrustedUsers[1].Password);
            Assert.IsNotNull(result2);
        }

        [Test]
        public void ChangeUserEmailAddress()
        {            
            // Get the user
            var userservice = new DomainUserService();
            var user = userservice.RetrieveUserByEmail(UserTestHelpers.TrustedUsers[2].Email);

            // Disapprove User
            var membershipService = new MembershipService();

            // Change the password
            var membershipUserService = new MembershipService();
            membershipUserService.ChangeEmailAddress(user, "bob@bob.com");

            // Try authenticating with valid credentials
            var admin2 = userservice.RetrieveUserByEmail("bob@bob.com");
            Assert.IsNotNull(admin2);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void ChangeUserEmailAddressAndTryToChangeAnotherAccountToSame()
        {
            // Get the user
            var userservice = new DomainUserService();
            var user = userservice.RetrieveUserByEmail(UserTestHelpers.TrustedUsers[2].Email);
            var otherUser = userservice.RetrieveUserByEmail(UserTestHelpers.TrustedUsers[3].Email);

            // Disapprove User
            var membershipService = new MembershipService();

            // Change the password
            var membershipUserService = new MembershipService();
            membershipUserService.ChangeEmailAddress(user, "bob@bob.com");

            // Try authenticating with valid credentials
            var admin2 = userservice.RetrieveUserByEmail("bob@bob.com");
            Assert.IsNotNull(admin2);

            membershipService.ChangeEmailAddress(otherUser, "bob@bob.com");
        }
        #endregion

        [TearDown]
        public void Teardown()
        {
            UserTestHelpers.CleanupTestUser();
        }
    }
}
