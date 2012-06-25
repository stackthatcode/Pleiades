using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Framework.MembershipProvider.Interface;
using Model = Pleiades.Framework.MembershipProvider.Model;
using Pleiades.Framework.MembershipProvider.Providers;

namespace Pleiades.Framework.UnitTests.Membership.Provider
{
    [TestFixture]
    public class MembershipProviderTests
    {
        PfMembershipProvider Provider;

        [SetUp]
        public void TestSetup()
        {
            Provider = new PfMembershipProvider();
            Provider.MembershipRepository = MockRepository.GenerateStub<IMembershipRepository>();
            PfMembershipProvider.MembershipProviderSettings = MockRepository.GenerateStub<MembershipProviderSettings>();
            PfMembershipProvider.MembershipProviderSettings.ProviderName = "My Provider";
        }

        [Test]
        public void CreateUser_Fails_With_DuplicateEmail() 
        {
            // Arrange
            var username = "1111111";
            var email = "aleks@test.com";
            PfMembershipProvider.MembershipProviderSettings.Expect(x => x.RequiresUniqueEmail).Return(true);
            Provider.MembershipRepository.Expect(x => x.GetUserNameByEmail(email)).Return(username);

            // Act
            MembershipCreateStatus statusOutput;
            var user = Provider.CreateUser(
                username, "password1", email, "Whawt's This?", "What it is", true, Guid.NewGuid(), out statusOutput);
            
            // Assert
            Assert.AreEqual(MembershipCreateStatus.DuplicateEmail, statusOutput);
            Assert.IsNull(user);
        }

        [Test]
        public void CreateUser_Fails_With_DuplicateUserName()
        {
            // Arrange
            var username = "1111111";
            var email = "aleks@test.com"; 
            PfMembershipProvider.MembershipProviderSettings.Expect(x => x.RequiresUniqueEmail).Return(true);
            Provider.MembershipRepository.Expect(x => x.GetUserNameByEmail(null)).IgnoreArguments().Return("");
            Provider.MembershipRepository.Expect(x => x.GetUser(username)).Return(new Model.MembershipUser());
            
            // Act
            MembershipCreateStatus statusOutput;
            var user = Provider.CreateUser(
                username, "password1", email, "Whawt's This?", "What it is", true, Guid.NewGuid(), out statusOutput);
            
            // Assert
            Assert.AreEqual(MembershipCreateStatus.DuplicateUserName, statusOutput);
            Assert.IsNull(user);
        }

        [Test]
        public void CreateUser_Fails_With_Invalid_Guid()
        {
            // Arrange
            var username = "1111111";
            var email = "aleks@test.com";
            PfMembershipProvider.MembershipProviderSettings.Expect(x => x.RequiresUniqueEmail).Return(true);
            Provider.MembershipRepository.Expect(x => x.GetUserNameByEmail(null)).IgnoreArguments().Return("");
            Provider.MembershipRepository.Expect(x => x.GetUser(username)).Return(null);

            // Act
            MembershipCreateStatus statusOutput;
            var user = Provider.CreateUser(
                username, "password1", email, "Whawt's This?", "What it is", true, "INVALID GUID", out statusOutput);

            // Assert
            Assert.AreEqual(MembershipCreateStatus.InvalidProviderUserKey, statusOutput);
            Assert.IsNull(user);
        }

        [Test]
        public void CreateUser_Fails_If_Repository_Throws()
        {
            // Arrange
            var username = "1111111";
            var email = "aleks@test.com";
            PfMembershipProvider.MembershipProviderSettings.Expect(x => x.RequiresUniqueEmail).Return(true);
            Provider.MembershipRepository.Expect(x => x.GetUserNameByEmail(null)).IgnoreArguments().Return("");
            Provider.MembershipRepository.Expect(x => x.GetUser(username)).Return(null);
            Provider.MembershipRepository
                    .Expect(x => x.Add(null)).IgnoreArguments()
                    .Do((Action<Model.MembershipUser>)((x) => { this.Throw(); }));

            // Act
            MembershipCreateStatus statusOutput;
            var user = Provider.CreateUser(
                username, "password1", email, "Whawt's This?", "What it is", true, Guid.NewGuid(), out statusOutput);

            // Assert
            Assert.AreEqual(MembershipCreateStatus.UserRejected, statusOutput);
            Assert.IsNull(user);
        }

        [Test]
        public void CreateUser_Succeeds()
        {
            // Arrange
            var username = "1111111";
            var email = "aleks@test.com";
            PfMembershipProvider.MembershipProviderSettings.Expect(x => x.RequiresUniqueEmail).Return(true);
            Provider.MembershipRepository.Expect(x => x.GetUserNameByEmail(null)).IgnoreArguments().Return("");
            Provider.MembershipRepository.Expect(x => x.GetUser(username)).Return(null).Repeat.Once();
            Provider.MembershipRepository.Expect(x => x.Add(null)).IgnoreArguments();
            Provider.MembershipRepository.Expect(x => x.SaveChanges());
            Provider.MembershipRepository.Expect(x => x.GetUser(username))
                .Return(new Model.MembershipUser()
                {
                    ProviderUserKey = Guid.NewGuid(),
                }).Repeat.Once();
            
            // Act
            MembershipCreateStatus statusOutput = MembershipCreateStatus.UserRejected;

            try
            {
                var user = Provider.CreateUser(
                    username, "password1", email, "Whawt's This?", "What it is", true, Guid.NewGuid(), out statusOutput);
            }
            catch
            {
                // should have thrown
            }

            // Assert
            Assert.AreEqual(MembershipCreateStatus.Success, statusOutput);
        }


        [Test]
        public void ChangePasswordQuestionAndAnswer_Invalid_Credentials_Fails()
        {
            // Arrange
            var username = "1111111";
            var email = "aleks@test.com";
            PfMembershipProvider.MembershipProviderSettings.Expect(x => x.RequiresUniqueEmail).Return(true);
            Provider.MembershipRepository.Expect(x => x.GetUser(username)).Return(null);


            // Act
            var result = Provider.ChangePasswordQuestionAndAnswer(username, "pass1", "Whatsdtgkn", "Potatoes");

            // Assert
            Assert.IsFalse(result);
        }

        public void Throw()
        {
            throw new Exception("I don't like you!"); 
        }
    }
}