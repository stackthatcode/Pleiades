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
    public class MembershipServiceWithQuestionTests
    {
        [SetUp]
        public void TestSetup()
        {
            UserTestHelpers.ResetTestUser();
        }

        #region Question & Answer Password Reset Tests
        [Test]
        public void ResetPasswordWithQuestionAndAnswerTest()
        {
            // Get the user
            var userservice = new DomainUserService();
            var admin = userservice.RetrieveUserByEmail(UserTestHelpers.AdminUsers[0].Email);

            // Reset the Password
            var membershipUserService = new MembershipService();
            var question = membershipUserService.PasswordQuestion(admin);
            Assert.AreEqual(question, UserTestHelpers.AdminUsers[0].PasswordQuestion);

            // Try Reseting
            var resetPwd = 
                membershipUserService.ResetPasswordWithAnswer(
                    admin, UserTestHelpers.AdminUsers[0].PasswordAnswer);

            // Try authenticating with valid credentials
            var result = 
                membershipUserService.ValidateUserByEmailAddr(
                    UserTestHelpers.AdminUsers[0].Email, resetPwd);

            Assert.IsNotNull(result);
        }


        /// <summary>
        /// REQUIRES CONFIGURATION SETTING MODIFICATION TO PASS
        /// </summary>
        [Test]
        [Ignore]
        [ExpectedException(typeof(MembershipPasswordException), Message = "Incorrect password answer.")]
        public void ResetPasswordWithQuestionAndAnswerTestFAIL()
        {
            // Get the user
            var userservice = new DomainUserService();
            var admin = userservice.RetrieveUserByEmail(UserTestHelpers.AdminUsers[0].Email);

            // Reset the Password with WRONG answer
            var membershipUserService = new MembershipService();
            var resetPwd =
                membershipUserService.ResetPasswordWithAnswer(admin, "Donald777");

            // Should've *thrown* an Exception
        }

        /// <summary>
        /// REQUIRES CONFIGURATION SETTING MODIFICATION TO PASS
        /// </summary>
        [Test]
        [Ignore]
        public void ChangePasswordQuestionAndAnswerTest()
        {
            // Get the user
            var userservice = new DomainUserService();
            var user = userservice.RetrieveUserByEmail(UserTestHelpers.TrustedUsers[2].Email);

            // Change the question and answer
            var membershipUserService = new MembershipService();
            membershipUserService
                .ChangePasswordQuestionAndAnswer(
                    user, 
                    UserTestHelpers.TrustedUsers[2].Email,
                    UserTestHelpers.TrustedUsers[2].PasswordQuestion, 
                    UserTestHelpers.TrustedUsers[2].PasswordAnswer);

            // Test the question
            var question = membershipUserService.PasswordQuestion(user);
            Assert.AreEqual(question, "My wife");

            // Try Reseting
            var resetPwd = membershipUserService.ResetPasswordWithAnswer(user, UserTestHelpers.TrustedUsers[2].PasswordAnswer);

            // Try authenticating with valid credentials
            var result = membershipUserService.ValidateUserByEmailAddr(UserTestHelpers.TrustedUsers[2].Email, resetPwd);
            Assert.IsNotNull(result);
        }
        #endregion

        [TearDown]
        public void Teardown()
        {
            UserTestHelpers.CleanupTestUser();
        }
    }
}
