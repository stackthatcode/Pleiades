using System.Configuration.Provider;
using System.Web.Security;
using Autofac;
using NUnit.Framework;
using Pleiades.Application.Data;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.IntegrationTests.Services
{
    /// <summary>
    /// Big fucking awful TODO - fix the lifetime scope stuff after killing the Membership Provider
    /// </summary>
    [TestFixture]
    public class MembershipRepositoryTests : FixtureBase
    {        
        [Test]
        public void Create_MembershipUser_Pass()
        {
            var lifetime = TestContainer.LifetimeScope();
            var service = lifetime.Resolve<IPfMembershipService>();
            var request = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones1@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            PfMembershipCreateStatus statusResponse;
            var newUser = service.CreateUser(request, out statusResponse);

            Assert.AreEqual(statusResponse, PfMembershipCreateStatus.Success);
            Assert.AreEqual(request.Email, newUser.Email);
            Assert.AreEqual(request.IsApproved, newUser.IsApproved);
        }

        [Test]
        public void Create_Approved_User_And_Authenticate_Pass()
        {
            var lifetime = TestContainer.LifetimeScope();
            var service = lifetime.Resolve<IPfMembershipService>();
            var request = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones2@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            PfMembershipCreateStatus statusResponse;
            var newUser = service.CreateUser(request, out statusResponse);

            Assert.AreEqual(statusResponse, PfMembershipCreateStatus.Success);

            var result = service.ValidateUserByEmailAddr(request.Email, request.Password);
            Assert.IsNotNull(result);
        }

        [Test]
        public void Create_Approved_User_And_Authenticate_WithBadPassword_Should_Fail()
        {
            var lifetime = TestContainer.LifetimeScope();
            var service = lifetime.Resolve<IPfMembershipService>();
            var request = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones3@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            PfMembershipCreateStatus statusResponse;
            var newUser = service.CreateUser(request, out statusResponse);

            Assert.AreEqual(statusResponse, PfMembershipCreateStatus.Success);

            var result = service.ValidateUserByEmailAddr(request.Email, "bullshit password");
            Assert.IsNull(result);
        }

        [Test]
        public void Create_Disapproved_User_And_Authenticate_WithGoodPassword_Should_Fail()
        {
            var lifetime = TestContainer.LifetimeScope();
            var service = lifetime.Resolve<IPfMembershipService>();
            var request = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones4@gmail.com",
                IsApproved = false,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            PfMembershipCreateStatus statusResponse;
            var newUser = service.CreateUser(request, out statusResponse);
            Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);

            Assert.AreEqual(statusResponse, PfMembershipCreateStatus.Success);

            var result = service.ValidateUserByEmailAddr(request.Email, request.Password);
            Assert.IsNull(result);
        }

        [Test]
        public void Create_User_And_Reset_Password_Fail_To_Authenticate_Then_Reset_Then_Authenticate_And_Pass()
        {
            var lifetime = TestContainer.LifetimeScope();
            var service = lifetime.Resolve<IPfMembershipService>();
            var request = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones5@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            PfMembershipCreateStatus statusResponse;
            var newUser = service.CreateUser(request, out statusResponse);
            Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);

            // Reset the Password
            PfCredentialsChangeStatus status;
            var resetPwd = service.ResetPassword(newUser.UserName, null, true, out status);

            // Try authenticating with valid credentials
            var result = service.ValidateUserByEmailAddr(newUser.Email, resetPwd);
            Assert.IsNotNull(result);

            // Try authenticating with invalid credentials
            var result2 = service.ValidateUserByEmailAddr(newUser.Email, "fgghjkgaaewgw");
            Assert.IsNull(result2);

            // Now, use the reset password to change the Password
            service.ChangePassword(newUser.UserName, resetPwd, "password1", true);
            var result3 = service.ValidateUserByEmailAddr(newUser.Email, "password1");
            Assert.IsNotNull(result3);
        }

        [Test]
        public void Disapprove_User_And_Validate_Fails_Reapprove_And_Validate_Succeeds()
        {
            var lifetime = TestContainer.LifetimeScope();
            var service = lifetime.Resolve<IPfMembershipService>();
            var unitOfWork = lifetime.Resolve<IUnitOfWork>();

            var request = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones6@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            PfMembershipCreateStatus statusResponse;
            var newUser = service.CreateUser(request, out statusResponse);
            Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);

            // Disapprove User
            service.SetUserApproval(newUser.UserName, false);
            unitOfWork.SaveChanges();

            // Try to Authenticate - should fail
            var result1 = service.ValidateUserByEmailAddr(newUser.Email, "password123");
            Assert.IsNull(result1);

            // Reapprove USer
            service.SetUserApproval(newUser.UserName, true);
            unitOfWork.SaveChanges();

            // Try to Authenticate - should succeed
            var result2 = service.ValidateUserByEmailAddr(newUser.Email, "password123");
            Assert.IsNotNull(result2);
        }

        [Test]
        public void Failed_Authentication_Locks_User_Out_And_Unlock_User_Enables_Validation()
        {
            var lifetime = TestContainer.LifetimeScope();
            var unitOfWork = lifetime.Resolve<IUnitOfWork>();
            var service = lifetime.Resolve<IPfMembershipService>();
            var request = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones7@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            PfMembershipCreateStatus statusResponse;
            var newUser = service.CreateUser(request, out statusResponse);
            Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);

            // Get user
            for (int i = 0; i < 10; i++)
            {
                service.ValidateUserByEmailAddr(newUser.Email, "wrongpassword");
            }

            // Should be locked out
            var result1 = service.ValidateUserByEmailAddr(newUser.Email, "password123");
            Assert.IsNull(result1);

            // Unlock the User
            service.UnlockUser(newUser.UserName);
            unitOfWork.SaveChanges();

            // Should be able to authenticated
            var result2 = service.ValidateUserByEmailAddr(newUser.Email, "password123");
            Assert.IsNotNull(result2);
        }

        [Test]
        public void Change_User_Email_And_Validate_Successfully()
        {
            var lifetime = TestContainer.LifetimeScope();
            var service = lifetime.Resolve<IPfMembershipService>();
            var unitOfWork = lifetime.Resolve<IUnitOfWork>();
            var request = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones8@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            PfMembershipCreateStatus statusResponse;
            var newUser = service.CreateUser(request, out statusResponse);
            Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);

            // Change the password
            service.ChangeEmailAddress(newUser.UserName, null, "bob4444@bob.com", true);
            unitOfWork.SaveChanges();

            // Try authenticating with valid credentials
            var result = service.ValidateUserByEmailAddr("bob4444@bob.com", "password123");
            Assert.IsNotNull(result);
        }

        [Test]
        [ExpectedException(typeof(ProviderException))]
        public void Change_User_Email_Address_To_Another_Users_Email_Address()
        {
            var lifetime = TestContainer.LifetimeScope();
            var service = lifetime.Resolve<IPfMembershipService>();
            var request1 = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones9@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            PfMembershipCreateStatus statusResponse;
            var newUser1 = service.CreateUser(request1, out statusResponse);
            Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);

            var request2 = new PfCreateNewMembershipUserRequest()
            {
                Email = "bob999@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "1123",
                PasswordQuestion = "fdgdfgdgf",
            };

            var newUser2 = service.CreateUser(request2, out statusResponse);
            Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);

            // Should throw an Exception
            service.ChangeEmailAddress(newUser1.UserName, null, "bob999@gmail.com", true);
        }

        [Test]
        public void Reset_Password_With_Question_And_Answer_Test()
        {
            var lifetime = TestContainer.LifetimeScope();
            var service = lifetime.Resolve<IPfMembershipService>();
            var unitOfWork = lifetime.Resolve<IUnitOfWork>();

            var request1 = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones10@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            PfMembershipCreateStatus statusResponse;
            var newUser1 = service.CreateUser(request1, out statusResponse);
            Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);

            var question = service.PasswordQuestion(newUser1.UserName);
            Assert.AreEqual(question, request1.PasswordQuestion);

            // Try Reseting
            PfCredentialsChangeStatus status;
            var resetPwd = service.ResetPassword(newUser1.UserName, request1.PasswordAnswer, true, out status);
            unitOfWork.SaveChanges();

            // Try authenticating with valid credentials
            var result = service.ValidateUserByEmailAddr(newUser1.Email, resetPwd);

            Assert.IsNotNull(result);
        }


        /// <summary>
        /// REQUIRES CONFIGURATION SETTING TO DEMAND QUESTION TO RESET PASSWORD
        /// </summary>
        [Test]
        [Ignore]
        [ExpectedException(typeof(MembershipPasswordException))]
        public void Reset_Password_With_Wrong_And_Answer_And_FAIL()
        {
            var lifetime = TestContainer.LifetimeScope();
            var service = lifetime.Resolve<IPfMembershipService>();
            var unitOfWork = lifetime.Resolve<IUnitOfWork>();

            var request1 = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones11@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            PfMembershipCreateStatus statusResponse;
            var newUser1 = service.CreateUser(request1, out statusResponse);
            Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);

            // Reset the Password with WRONG answer
            PfCredentialsChangeStatus status;
            var resetPwd = service.ResetPassword(request1.Email, null, true, out status);
            unitOfWork.SaveChanges();

            // Should've *thrown* an Exception
        }

        /// <summary>
        /// REQUIRES CONFIGURATION SETTING TO DEMAND QUESTION TO RESET PASSWORD
        /// </summary>
        [Test]
        public void ChangePasswordQuestionAndAnswerTest()
        {
            var lifetime = TestContainer.LifetimeScope();
            var service = lifetime.Resolve<IPfMembershipService>();
            var unitOfWork = lifetime.Resolve<IUnitOfWork>();

            var request1 = new PfCreateNewMembershipUserRequest()
            {
                Email = "aleksjones12@gmail.com",
                IsApproved = true,
                Password = "password123",
                PasswordAnswer = "You",
                PasswordQuestion = "Who dat?",
            };

            PfMembershipCreateStatus statusResponse;
            var newUser1 = service.CreateUser(request1, out statusResponse);
            Assert.AreEqual(PfMembershipCreateStatus.Success, statusResponse);

            // Change the question and answer
            service.ChangePasswordQuestionAndAnswer(newUser1.UserName, "password123", "New Question", "New Answer", false);
            unitOfWork.SaveChanges();

            // Test the question
            var question = service.PasswordQuestion(newUser1.UserName);
            unitOfWork.SaveChanges();
            Assert.AreEqual(question, "New Question");

            // Try Reseting
            PfCredentialsChangeStatus status;
            var resetPwd = service.ResetPassword(newUser1.UserName, "New Answer", true, out status);
            unitOfWork.SaveChanges();

            // Try authenticating with valid credentials
            var result = service.ValidateUserByEmailAddr(newUser1.Email, resetPwd);
            Assert.IsNotNull(result);
        }
    }
}
