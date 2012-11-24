using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Tests.Security.IntegrationTests
{
    [TestFixture]
    public class AggregateUserServiceCreateTests
    {
        #region Create User method tests
        [Test]
        [ExpectedException(typeof(Exception))]
        public void Verify_Cant_Add_More_Than_Maximum_Number_Of_Admins()
        {
            // Arrange
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            var aggregateUserService = new  AggregateUserService(null, repository, null, null, null);
            repository.Expect(x => x.GetUserCountByRole(UserRole.Admin)).Return(AggregateUserService.MaxAdminUsers);

            // Act
            PleiadesMembershipCreateStatus createStatus;
            aggregateUserService.Create(
                null, 
                new CreateOrModifyIdentityRequest() { UserRole = UserRole.Admin }, 
                out createStatus);

            // Assert - should throw!            
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Verify_Cant_Add_More_Than_Maximum_Number_Of_Supreme_Users()
        {
            // Arrange
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            var aggregateUserService = new AggregateUserService(null, repository, null, null, null);
            repository.Expect(x => x.GetUserCountByRole(UserRole.Admin)).Return(1);
            repository.Expect(x => x.GetUserCountByRole(UserRole.Supreme)).Return(AggregateUserService.MaxSupremeUsers);

            // Act
            PleiadesMembershipCreateStatus createStatus;
            aggregateUserService.Create(
                null,
                new CreateOrModifyIdentityRequest() { UserRole = UserRole.Supreme },
                out createStatus);

            // Assert - should throw!            
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void Verify_Cant_Add_Anonymous_User()
        {
            // Arrange            
            var request = new CreateOrModifyIdentityRequest()
            {
                UserRole = UserRole.Anonymous,
                AccountLevel = AccountLevel.Gold,
                AccountStatus = AccountStatus.Active,
                FirstName = "Jon",
                LastName = "Smith",
            };
            var aggregateUserService = new AggregateUserService(null, null, null, null, null);

            // Act
            PleiadesMembershipCreateStatus createStatus;
            aggregateUserService.Create(null, request, out createStatus);

            // Assert - show thorw!
        }

        [Test]
        public void Verify_MembershipFailure_Create_Request()
        {
            // Arrange            
            var request = new CreateOrModifyIdentityRequest()
            {
                UserRole = UserRole.Trusted,
                AccountLevel = AccountLevel.Gold,
                AccountStatus = AccountStatus.Active,
                FirstName = "Jon",
                LastName = "Smith",
            };

            // Arrange
            PleiadesMembershipCreateStatus createStatus;
            var membership = MockRepository.GenerateMock<IMembershipService>();
            membership.Expect(x => x
                .CreateUser(null, out createStatus))
                .IgnoreArguments()
                .OutRef(PleiadesMembershipCreateStatus.DuplicateUserName);

            var aggregateUserService = new AggregateUserService(membership, null, null, null, null);

            // Act
            var response =
                aggregateUserService.Create(
                    null,
                    new CreateOrModifyIdentityRequest() { UserRole = UserRole.Trusted },
                    out createStatus);

            // Assert
            membership.VerifyAllExpectations();
            Assert.AreNotEqual(PleiadesMembershipCreateStatus.Success, createStatus);
            Assert.IsNull(response);
        }

        [Test]
        public void Verify_Valid_Create_Request()
        {
            // Arrange            
            var request = new CreateOrModifyIdentityRequest()
            {
                UserRole = UserRole.Trusted,
                AccountLevel = AccountLevel.Gold,
                AccountStatus = AccountStatus.Active,
                FirstName = "Jon",
                LastName = "Smith",
            };

            // Arrange
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository.Expect(x => x.Add(null)).IgnoreArguments();
            repository.Expect(x => x.SaveChanges());
            
            PleiadesMembershipCreateStatus createStatus;
            var membership = MockRepository.GenerateMock<IMembershipService>();
            membership.Expect(x => x.CreateUser(null, out createStatus)).IgnoreArguments().OutRef(PleiadesMembershipCreateStatus.Success);

            // Act
            var aggregateUserService = new AggregateUserService(membership, repository, null, null, null);

            var response = aggregateUserService.Create(
                    null,
                    new CreateOrModifyIdentityRequest() 
                    {
                        AccountStatus = AccountStatus.Active,
                        AccountLevel = AccountLevel.Gold,
                        UserRole = UserRole.Trusted 
                    },
                    out createStatus);

            // Assert
            repository.VerifyAllExpectations();
            membership.VerifyAllExpectations();
            Assert.IsNotNull(response);
        }
        #endregion


        #region Update Identity method testing
        [Test]
        public void Verify_Approved_Owners_Calling_UpdateIdentity_Successfully_Invokes_Services()
        {
            // Arrange
            var user = new AggregateUser()
            {
                ID = 888,
                IdentityProfile = new IdentityProfile()
                {
                    UserRole = UserRole.Admin,
                },
                Membership = new MembershipUser
                {
                    UserName = "12345678",
                },
            };

            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityCode.Allowed);

            var aggregateUserRepository = MockRepository.GenerateMock<IAggregateUserRepository>();
            aggregateUserRepository.Expect(x => x.UpdateIdentity(888, null)).IgnoreArguments();

            var aggregateService = new AggregateUserService(null, aggregateUserRepository, ownerAuthorizationService, null, null);

            // Act
            aggregateService.UpdateIdentity(888,
                new CreateOrModifyIdentityRequest() { FirstName = "Jospeh", LastName = "Lambert" });

            // Assert
            ownerAuthorizationService.VerifyAllExpectations();
            aggregateUserRepository.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException()]
        public void Verify_Unapproved_Owners_Calling_UpdateIdentity_Throws()
        {
            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityCode.AccessDenied);

            var aggregateService = new AggregateUserService(null, null, ownerAuthorizationService, null, null);

            // Act
            aggregateService.UpdateIdentity(888,
                new CreateOrModifyIdentityRequest() { FirstName = "Jospeh", LastName = "Lambert" });

            // Assert
            ownerAuthorizationService.VerifyAllExpectations();
        }

        [Test]
        public void Verify_Update_Email_Method_Pass()
        {
            // Arrange
            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityCode.Allowed);

            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository
                .Expect(x => x.RetrieveById(888))
                .Return(new AggregateUser { Membership = new MembershipUser { UserName = "123456" } });

            var memebrship = MockRepository.GenerateMock<IMembershipService>();
            memebrship.Expect(x => x.ChangeEmailAddress("123456", "john@john.com"));

            // Act
            var aggregateService = new AggregateUserService(memebrship, repository, ownerAuthorizationService, null, null);
            aggregateService.UpdateEmail(888, "john@john.com");

            // Assert
            ownerAuthorizationService.VerifyAllExpectations();
            repository.VerifyAllExpectations();
            memebrship.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException()]
        public void Verify_Update_Email_Method_Throws_For_Unapproved_Users()
        {
            // Arrange
            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityCode.AccessDenied);

            // Act
            var aggregateService = new AggregateUserService(null, null, ownerAuthorizationService, null, null);
            aggregateService.UpdateEmail(888, "john@john.com");

            // Assert
        }

        [Test]
        [ExpectedException()]
        public void Verify_Update_Email_Method_Throws_For_Null_Email_Address()
        {
            // Arrange
            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityCode.AccessDenied);

            // Act
            var aggregateService = new AggregateUserService(null, null, ownerAuthorizationService, null, null);
            aggregateService.UpdateEmail(888, null);

            // Assert
        }

        [Test]
        public void Verify_UpdateApproval_HappyPath()
        {
            // Arrange
            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityCode.Allowed);

            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository
                .Expect(x => x.RetrieveById(888))
                .Return(new AggregateUser
                {
                    Membership = new MembershipUser { UserName = "123456" },
                    IdentityProfile = new IdentityProfile { UserRole = UserRole.Trusted },
                });

            var memebrship = MockRepository.GenerateMock<IMembershipService>();
            memebrship.Expect(x => x.SetUserApproval("123456", true));

            // Act
            var aggregateService = new AggregateUserService(memebrship, repository, ownerAuthorizationService, null, null);
            aggregateService.UpdateApproval(888, true);

            // Assert
            ownerAuthorizationService.VerifyAllExpectations();
            repository.VerifyAllExpectations();
            memebrship.VerifyAllExpectations();
        }

        [Test]
        public void Verify_UpdateApproval_Cannot_Update_SupremeUser()
        {
            // Arrange
            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityCode.Allowed);

            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository
                .Expect(x => x.RetrieveById(888))
                .Return(new AggregateUser
                {
                    Membership = new MembershipUser { UserName = "123456" },
                    IdentityProfile = new IdentityProfile { UserRole = UserRole.Supreme },
                });

            // Act
            var aggregateService = new AggregateUserService(null, repository, ownerAuthorizationService, null, null);
            aggregateService.UpdateApproval(888, true);

            // Assert
            ownerAuthorizationService.VerifyAllExpectations();
            repository.VerifyAllExpectations();
        }

        [Test]
        public void Verify_Update_Password_Properly_Invokes_Services()
        {
            // Arrange
            var user = new AggregateUser()
            {
                IdentityProfile = new IdentityProfile(),
                Membership = new MembershipUser
                {
                    UserName = "12345678",
                }
            };

            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository.Expect(x => x.RetrieveById(888)).Return(user);

            var membershipService = MockRepository.GenerateMock<IMembershipService>();
            membershipService.Expect(x => x.ChangePassword("12345678", "12345678", "abcdef"));

            var service = new AggregateUserService(membershipService, repository, null, null, null);

            // Act
            service.ChangeUserPassword(888, "12345678", "abcdef");

            // Assert
            membershipService.VerifyAllExpectations();
        }
        #endregion
    }
}