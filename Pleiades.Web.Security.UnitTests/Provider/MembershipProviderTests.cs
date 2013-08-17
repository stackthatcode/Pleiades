﻿using System;
using System.Web.Security;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Providers;

namespace Pleiades.Web.Security.UnitTests.Provider
{
    [TestFixture]
    public class MembershipProviderTests
    {
        public IMembershipProviderRepository ProviderRepositoryInjector(PfMembershipProvider provider)
        {
            var repository = MockRepository.GenerateStub<IMembershipProviderRepository>();
            PfMembershipRepositoryBroker.RegisterFactory(() => repository);
            return repository;
        }


        [Test]
        public void CreateUser_Fails_With_DuplicateEmail() 
        {
            // Arrange
            var provider = new PfMembershipProvider();            
            var username = "1111111";
            var email = "aleks@test.com";

            var settings = new PfMembershipProviderSettings()
            {
                RequiresUniqueEmail = true
            };
            provider.Settings = settings;
            
            var repository = ProviderRepositoryInjector(provider);
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
            var provider = new PfMembershipProvider() { Settings = new PfMembershipProviderSettings() };
            provider.Settings.RequiresUniqueEmail = true;
            var username = "1111111";
            var email = "aleks@test.com";

            var repository = ProviderRepositoryInjector(provider);
            repository.Expect(x => x.GetUserNameByEmail(null)).IgnoreArguments().Return("");
            repository.Expect(x => x.GetUser(username)).Return(new Model.PfMembershipUser());

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
            var provider = new PfMembershipProvider() { Settings = new PfMembershipProviderSettings() };
            provider.Settings.RequiresUniqueEmail = true;

            var username = "1111111";
            var email = "aleks@test.com";

            var repository = ProviderRepositoryInjector(provider); 
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
            var provider = new PfMembershipProvider() { Settings = new PfMembershipProviderSettings() };
            provider.Settings.RequiresUniqueEmail = true;
            var username = "1111111";
            var email = "aleks@test.com";

            var repository = ProviderRepositoryInjector(provider);
            repository.Expect(x => x.GetUserNameByEmail(null)).IgnoreArguments().Return("");
            repository.Expect(x => x.GetUser(username)).Return(null);
            repository
                    .Expect(x => x.Insert(null)).IgnoreArguments()
                    .Do((Action<Model.PfMembershipUser>)((x) => { this.Throw(); }));

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
            var provider = new PfMembershipProvider() { Settings = new PfMembershipProviderSettings() };
            provider.Settings.RequiresUniqueEmail = true;

            var username = "1111111";
            var email = "aleks@test.com";

            var repository = ProviderRepositoryInjector(provider);
            repository.Expect(x => x.GetUserNameByEmail(null)).IgnoreArguments().Return("");
            repository.Expect(x => x.GetUser(username)).Return(null).Repeat.Once();
            repository.Expect(x => x.Insert(null)).IgnoreArguments();
            repository.Expect(x => x.GetUser(username))
                .Return(new Model.PfMembershipUser()
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
            var provider = new PfMembershipProvider() { Settings = new PfMembershipProviderSettings() };
            provider.Settings.RequiresUniqueEmail = true;
            var username = "1111111";
            var email = "aleks@test.com";
            var repository = ProviderRepositoryInjector(provider);
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