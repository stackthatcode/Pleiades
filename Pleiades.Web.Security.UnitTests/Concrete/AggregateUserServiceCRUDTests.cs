using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Data;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Tests.Security.IntegrationTests
{
    [TestFixture]
    public class AggregateUserServiceCreateTests
    {
        #region CreateUser tests
        [Test]
        [ExpectedException(typeof(Exception))]
        public void Cant_Add_More_Than_Maximum_Number_Of_Admins()
        {
            // Arrange
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            var aggregateUserService = new  AggregateUserService(null, repository, null, null, null, null);
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
        public void Cant_Add_More_Than_Maximum_Number_Of_Supreme_Users()
        {
            // Arrange
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            var aggregateUserService = new AggregateUserService(null, repository, null, null, null, null);
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
        public void Cant_Add_Anonymous_User()
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
            var aggregateUserService = new AggregateUserService(null, null, null, null, null, null);

            // Act
            PleiadesMembershipCreateStatus createStatus;
            aggregateUserService.Create(null, request, out createStatus);

            // Assert - show thorw!
        }

        [Test]
        public void MembershipFailure_Create_Request()
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

            var aggregateUserService = new AggregateUserService(membership, null, null, null, null, null);

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
        public void Valid_Create_Request_Passes()
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

            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            unitOfWork.Expect(x => x.Commit());

            PleiadesMembershipCreateStatus createStatus;
            var membership = MockRepository.GenerateMock<IMembershipService>();
            membership.Expect(x => x.CreateUser(null, out createStatus)).IgnoreArguments().OutRef(PleiadesMembershipCreateStatus.Success);

            // Act
            var aggregateUserService = new AggregateUserService(membership, repository, null, null, null, unitOfWork);

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
            unitOfWork.VerifyAllExpectations();
            Assert.IsNotNull(response);
        }
        #endregion

        #region Update  tests
        [Test]
        public void Authorized_Users_Calling_UpdateIdentity_Suceeds()
        {
            // Arrange
            var user = new AggregateUser()
            {
                ID = 888,
                IdentityProfile = new IdentityProfile() { UserRole = UserRole.Admin, },
                Membership = new MembershipUser { UserName = "12345678", },
            };

            var modifyRequest = new CreateOrModifyIdentityRequest() 
                    { Id = 888, FirstName = "Jospeh", LastName = "Lambert", Email = "abc@efe.com", IsApproved = true };

            var aggrUserRepository = MockRepository.GenerateMock<IAggregateUserRepository>();
            aggrUserRepository.Expect(x => x.RetrieveById(888)).Return(user);
            aggrUserRepository.Expect(x => x.UpdateIdentity(modifyRequest));

            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityCode.Allowed);

            var membershipService = MockRepository.GenerateMock<IMembershipService>();
            membershipService.Expect(x => x.SetUserApproval("123456", true));
            membershipService.Expect(x => x.ChangeEmailAddress("123456", "abc@efe.com"));

            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            unitOfWork.Expect(x => x.Commit());

            var aggregateService =
                new AggregateUserService(membershipService, aggrUserRepository, ownerAuthorizationService, null, null, unitOfWork);

            // Act
            aggregateService.UpdateIdentity(modifyRequest);

            // Assert
            ownerAuthorizationService.VerifyAllExpectations();
            aggrUserRepository.VerifyAllExpectations();
            unitOfWork.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException()]
        public void Unauthorized_Owners_Calling_UpdateIdentity_Fails()
        {
            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityCode.AccessDenied);

            var aggregateService = new AggregateUserService(null, null, ownerAuthorizationService, null, null, null);

            // Act
            aggregateService.UpdateIdentity(new CreateOrModifyIdentityRequest() 
                    { Id = 888, FirstName = "Jospeh", LastName = "Lambert", Email = "abc@efe.com", IsApproved = true });

            // Assert
            ownerAuthorizationService.VerifyAllExpectations();
        }
        #endregion

        #region ChangePassword tests
        [Test]
        public void Authorized_Users_Calling_Update_Password_Succeeds()
        {
            // Arrange
            var user = new AggregateUser()
            {
                IdentityProfile = new IdentityProfile(),
                Membership = new MembershipUser { UserName = "12345678", }
            };

            var membershipService = MockRepository.GenerateMock<IMembershipService>();
            membershipService.Expect(x => x.ChangePassword("12345678", "12345678", "abcdef"));
            
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository.Expect(x => x.RetrieveById(888)).Return(user);

            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityCode.Allowed);

            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            unitOfWork.Expect(x => x.Commit());

            var service = new AggregateUserService(membershipService, repository, ownerAuthorizationService, null, null, unitOfWork);

            // Act
            service.ChangeUserPassword(888, "12345678", "abcdef");

            // Assert
            ownerAuthorizationService.VerifyAllExpectations();
            repository.VerifyAllExpectations();
            membershipService.VerifyAllExpectations();
            unitOfWork.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException()]
        public void UnAuthorized_Users_Calling_Update_Password_Fails()
        {
            // Arrange
            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityCode.AccessDenied);

            var service = new AggregateUserService(null, null, ownerAuthorizationService, null, null, null);

            // Act
            service.ChangeUserPassword(888, "12345678", "abcdef");

            // Assert
            ownerAuthorizationService.VerifyAllExpectations();
        }
        #endregion
    }
}