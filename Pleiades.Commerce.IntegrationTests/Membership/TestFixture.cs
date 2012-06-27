using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using NUnit.Framework;
using Pleiades.Commerce.Persist;
using Pleiades.Framework.MembershipProvider.Concrete;
using Pleiades.Framework.MembershipProvider.Model;

namespace Pleiades.Framework.IntegrationTests.Membership
{
    [TestFixture]
    public class TestFixture
    {
        [SetUp]
        public void TestSetup()
        {
            var context = new MembershipContextForTesting();
            if (context.Database.Exists())
            {
                context.Database.Delete();
            }
            context.Database.Create();

            var repository = new MembershipRepository(context);
            RepositoryShim.GetInstance = () => repository;
        }

        [Test]
        public void Create_MembershipUser_Test()
        {
            var service = new MembershipService();
            var request = new CreateNewMembershipUserRequest()
            {
                Email = "aleksjones@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            MembershipCreateStatus statusResponse;
            var newUser = service.CreateUser(request, out statusResponse);

            Assert.AreEqual(statusResponse, MembershipCreateStatus.Success);
            Assert.AreEqual(request.Email, newUser.Email);
            Assert.AreEqual(request.IsApproved, newUser.IsApproved);
        }

        [Test]
        public void Create_Approved_User_And_Authenticate_Should_Succeed()
        {
            var service = new MembershipService();
            var request = new CreateNewMembershipUserRequest()
            {
                Email = "aleksjones2@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            MembershipCreateStatus statusResponse;
            var newUser = service.CreateUser(request, out statusResponse);

            Assert.AreEqual(statusResponse, MembershipCreateStatus.Success);

            var result = service.ValidateUserByEmailAddr(request.Email, request.Password);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Create_Approved_User_And_Authenticate_WithBadPassword_Should_Fail()
        {
            var service = new MembershipService();
            var request = new CreateNewMembershipUserRequest()
            {
                Email = "aleksjones3@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            MembershipCreateStatus statusResponse;
            var newUser = service.CreateUser(request, out statusResponse);

            Assert.AreEqual(statusResponse, MembershipCreateStatus.Success);

            var result = service.ValidateUserByEmailAddr(request.Email, "bullshit password");
            Assert.IsNull(result);
        }

        [Test]
        public void Create_Disapproved_User_And_Authenticate_WithGoodPassword_Should_Fail()
        {
            var service = new MembershipService();
            var request = new CreateNewMembershipUserRequest()
            {
                Email = "aleksjones3@gmail.com",
                IsApproved = false,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",                
            };

            MembershipCreateStatus statusResponse;
            var newUser = service.CreateUser(request, out statusResponse);

            Assert.AreEqual(statusResponse, MembershipCreateStatus.Success);

            var result = service.ValidateUserByEmailAddr(request.Email, request.Password);
            Assert.IsNull(result);
        }

        [Test]
        public void Create_User_And_Reset_Password_Fail_To_Authenticate_Then_Reset_Then_Authenticate_And_Pass()
        {
            var service = new MembershipService();
            var request = new CreateNewMembershipUserRequest()
            {
                Email = "aleksjones4@gmail.com",
                IsApproved = false,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            MembershipCreateStatus statusResponse;
            var newUser = service.CreateUser(request, out statusResponse);

            // Reset the Password
            var resetPwd = service.ResetPassword(newUser.UserName);

            // Try authenticating with valid credentials
            var result = service.ValidateUserByEmailAddr(newUser.Email, resetPwd);
            Assert.IsNotNull(result);

            // Try authenticating with invalid credentials
            var result2 = service.ValidateUserByEmailAddr(newUser.Email, "fgghjkgaaewgw");
            Assert.IsNull(result2);

            // Now, use the reset password to change the Password
            service.ChangePassword(newUser.UserName, resetPwd, "password1");
            var result3 = service.ValidateUserByEmailAddr(newUser.Email, "password1");
            Assert.IsNotNull(result3);
        }


        //[Test]
        //public void DisapproveAndApproveTesting()
        //{
        //    // Get the user
        //    var userservice = new DomainUserService();
        //    var user = userservice.RetrieveUserByEmail(UserTestHelpers.TrustedUsers[0].Email);

        //    // Disapprove User
        //    var membershipService = new MembershipService();
        //    membershipService.SetUserApproval(user, false);

        //    // Try to Authenticate - should fail
        //    var result1 = membershipService.ValidateUserByEmailAddr(
        //        UserTestHelpers.TrustedUsers[0].Email, UserTestHelpers.TrustedUsers[0].Password);
        //    Assert.IsNull(result1);

        //    // Reapprove USer
        //    membershipService.SetUserApproval(user, true);

        //    // Try to Authenticate - should succeed
        //    var result2 = membershipService.ValidateUserByEmailAddr(
        //        UserTestHelpers.TrustedUsers[0].Email, UserTestHelpers.TrustedUsers[0].Password);
        //    Assert.IsNotNull(result2);
        //}

        //[Test]
        //public void SimulateAccountLockoutAndUnlocking()
        //{
        //    // Get the user
        //    var userservice = new DomainUserService();
        //    var user = userservice.RetrieveUserByEmail(UserTestHelpers.TrustedUsers[1].Email);

        //    // Disapprove User
        //    var membershipService = new MembershipService();

        //    // Get user
        //    for (int i = 0; i < 10; i++)
        //    {
        //        membershipService.ValidateUserByEmailAddr(
        //            UserTestHelpers.TrustedUsers[1].Email, "wrongpassword");
        //    }
            
        //    // Should be locked out
        //    var result1 = membershipService.ValidateUserByEmailAddr(
        //        UserTestHelpers.TrustedUsers[1].Email, UserTestHelpers.TrustedUsers[1].Password);
        //    Assert.IsNull(result1);

        //    // Unlock the User
        //    membershipService.UnlockUser(user);
            
        //    // Should be able to authenticated
        //    var result2 = membershipService.ValidateUserByEmailAddr(
        //        UserTestHelpers.TrustedUsers[1].Email, UserTestHelpers.TrustedUsers[1].Password);
        //    Assert.IsNotNull(result2);
        //}

        //[Test]
        //public void ChangeUserEmailAddress()
        //{            
        //    // Get the user
        //    var userservice = new DomainUserService();
        //    var user = userservice.RetrieveUserByEmail(UserTestHelpers.TrustedUsers[2].Email);

        //    // Disapprove User
        //    var membershipService = new MembershipService();

        //    // Change the password
        //    var membershipUserService = new MembershipService();
        //    membershipUserService.ChangeEmailAddress(user, "bob@bob.com");

        //    // Try authenticating with valid credentials
        //    var admin2 = userservice.RetrieveUserByEmail("bob@bob.com");
        //    Assert.IsNotNull(admin2);
        //}

        //[Test]
        //[ExpectedException(typeof(Exception))]
        //public void ChangeUserEmailAddressAndTryToChangeAnotherAccountToSame()
        //{
        //    // Get the user
        //    var userservice = new DomainUserService();
        //    var user = userservice.RetrieveUserByEmail(UserTestHelpers.TrustedUsers[2].Email);
        //    var otherUser = userservice.RetrieveUserByEmail(UserTestHelpers.TrustedUsers[3].Email);

        //    // Disapprove User
        //    var membershipService = new MembershipService();

        //    // Change the password
        //    var membershipUserService = new MembershipService();
        //    membershipUserService.ChangeEmailAddress(user, "bob@bob.com");

        //    // Try authenticating with valid credentials
        //    var admin2 = userservice.RetrieveUserByEmail("bob@bob.com");
        //    Assert.IsNotNull(admin2);

        //    membershipService.ChangeEmailAddress(otherUser, "bob@bob.com");
        //}
        //#endregion
    }
}
