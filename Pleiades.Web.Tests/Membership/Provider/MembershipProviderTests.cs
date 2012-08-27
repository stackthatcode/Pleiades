using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Web.Security.Interface;
using Model = Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Providers;

namespace Pleiades.UnitTests.Membership.Provider
{
    [TestFixture]
    public class MembershipProviderTests
    {
        public IMembershipRepository ProviderRepositoryInjector(PfMembershipProvider provider)
        {
            PfMembershipProvider.MembershipProviderSettings = MockRepository.GenerateStub<MembershipProviderSettings>();
            PfMembershipProvider.MembershipProviderSettings.ProviderName = "My Provider";
            var repository = MockRepository.GenerateStub<IMembershipRepository>();
            provider.RepositoryFactory = () => repository;
            return repository;
        }


        [Test]
        public void CreateUser_Fails_With_DuplicateEmail() 
        {
            // Arrange
            var provider = new PfMembershipProvider();
            var repository = ProviderRepositoryInjector(provider);

            var username = "1111111";
            var email = "aleks@test.com";
            PfMembershipProvider.MembershipProviderSettings.Expect(x => x.RequiresUniqueEmail).Return(true);
            repository.Expect(x => x.GetUserNameByEmail(email)).Return(username);

            // Act
            MembershipCreateStatus statusOutput;
            var user = provider.CreateUser(
                username, "password1", email, "Whawt's This?", "What it is", true, Guid.NewGuid(), out statusOutput);
            
            // Assert
            Assert.AreEqual(MembershipCreateStatus.DuplicateEmail, statusOutput);
            Assert.IsNull(user);
        }

        [Test]
        public void CreateUser_Fails_With_DuplicateUserName()
        {
            // Arrange
            var provider = new PfMembershipProvider();
            var repository = ProviderRepositoryInjector(provider);

            var username = "1111111";
            var email = "aleks@test.com";
            PfMembershipProvider.MembershipProviderSettings.Expect(x => x.RequiresUniqueEmail).Return(true);
            repository.Expect(x => x.GetUserNameByEmail(null)).IgnoreArguments().Return("");
            repository.Expect(x => x.GetUser(username)).Return(new Model.MembershipUser());

            // Act
            MembershipCreateStatus statusOutput;
            var user = provider.CreateUser(
                username, "password1", email, "Whawt's This?", "What it is", true, Guid.NewGuid(), out statusOutput);

            // Assert
            Assert.AreEqual(MembershipCreateStatus.DuplicateUserName, statusOutput);
            Assert.IsNull(user);
        }

        [Test]
        public void CreateUser_Fails_With_Invalid_Guid()
        {
            // Arrange
            var provider = new PfMembershipProvider();
            var repository = ProviderRepositoryInjector(provider); 
            var username = "1111111";
            var email = "aleks@test.com";
            PfMembershipProvider.MembershipProviderSettings.Expect(x => x.RequiresUniqueEmail).Return(true);
            repository.Expect(x => x.GetUserNameByEmail(null)).IgnoreArguments().Return("");
            repository.Expect(x => x.GetUser(username)).Return(null);

            // Act
            MembershipCreateStatus statusOutput;
            var user = provider.CreateUser(
                username, "password1", email, "Whawt's This?", "What it is", true, "INVALID GUID", out statusOutput);

            // Assert
            Assert.AreEqual(MembershipCreateStatus.InvalidProviderUserKey, statusOutput);
            Assert.IsNull(user);
        }

        [Test]
        public void CreateUser_Fails_If_Repository_Throws()
        {
            // Arrange
            var provider = new PfMembershipProvider();
            var repository = ProviderRepositoryInjector(provider);
            var username = "1111111";
            var email = "aleks@test.com";
            PfMembershipProvider.MembershipProviderSettings.Expect(x => x.RequiresUniqueEmail).Return(true);
            repository.Expect(x => x.GetUserNameByEmail(null)).IgnoreArguments().Return("");
            repository.Expect(x => x.GetUser(username)).Return(null);
            repository
                    .Expect(x => x.Add(null)).IgnoreArguments()
                    .Do((Action<Model.MembershipUser>)((x) => { this.Throw(); }));

            // Act
            MembershipCreateStatus statusOutput;
            var user = provider.CreateUser(
                username, "password1", email, "Whawt's This?", "What it is", true, Guid.NewGuid(), out statusOutput);

            // Assert
            Assert.AreEqual(MembershipCreateStatus.UserRejected, statusOutput);
            Assert.IsNull(user);
        }

        [Test]
        public void CreateUser_Succeeds()
        {
            // Arrange
            var provider = new PfMembershipProvider();
            var repository = ProviderRepositoryInjector(provider); 
            var username = "1111111";
            var email = "aleks@test.com";
            PfMembershipProvider.MembershipProviderSettings.Expect(x => x.RequiresUniqueEmail).Return(true);
            repository.Expect(x => x.GetUserNameByEmail(null)).IgnoreArguments().Return("");
            repository.Expect(x => x.GetUser(username)).Return(null).Repeat.Once();
            repository.Expect(x => x.Add(null)).IgnoreArguments();
            repository.Expect(x => x.SaveChanges());
            repository.Expect(x => x.GetUser(username))
                .Return(new Model.MembershipUser()
                {
                    ProviderUserKey = Guid.NewGuid(),
                }).Repeat.Once();

            // Act
            MembershipCreateStatus statusOutput = MembershipCreateStatus.UserRejected;

            try
            {
                var user = provider.CreateUser(
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
            var provider = new PfMembershipProvider();
            var repository = ProviderRepositoryInjector(provider); 
            var username = "1111111";
            var email = "aleks@test.com";
            PfMembershipProvider.MembershipProviderSettings.Expect(x => x.RequiresUniqueEmail).Return(true);
            repository.Expect(x => x.GetUser(username)).Return(null);


            // Act
            var result = provider.ChangePasswordQuestionAndAnswer(username, "pass1", "Whatsdtgkn", "Potatoes");

            // Assert
            Assert.IsFalse(result);
        }

        public void Throw()
        {
            throw new Exception("I am an Exception!");
        }
    }
}