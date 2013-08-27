using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pleiades.Application.Data;
using Pleiades.TestHelpers.Web;
using Pleiades.Web.Security.Utility;
using Rhino.Mocks;
using Pleiades.Application;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.UnitTests.Concrete
{
    [TestFixture]
    public class AggregateUserServiceTests
    {
        [Test]
        public void Invalid_Credentials_Returns_Bad_Execution_State_And_Clears_Cookie()
        {
            // Arrange
            var membership = MockRepository.GenerateMock<IMembershipService>();
            membership.Expect(x => x.ValidateUserByEmailAddr("admin", "123")).Return(null);

            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            formsAuthService.Expect(x => x.ClearAuthenticationCookie());

            var service = new AggregateUserService(membership, null, formsAuthService, null);

            // Act
            var result = service.Authenticate("admin", "123", true, null);

            // Assert
            membership.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            Assert.IsFalse(result);
        }

        [Test]
        public void Valid_Credentials_With_The_Wrong_UserRole_Returns_Bad_Execution_State_And_Clears_Cookie()
        {
            // Arrange
            var membershipUser = new PfMembershipUser() { UserName = "12345678" };

            var membership = MockRepository.GenerateMock<IMembershipService>();
            membership.Expect(x => x.ValidateUserByEmailAddr("admin", "123")).Return(membershipUser);

            var aggrUser = new AggregateUser
            {
                IdentityProfile = new IdentityProfile { UserRole = UserRole.Trusted },
                Membership = membershipUser,
            };

            var aggregateRepository = MockRepository.GenerateMock<IAggregateUserRepository>();
            aggregateRepository.Expect(x => x.RetrieveByMembershipUserName("12345678")).Return(aggrUser);

            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            formsAuthService.Expect(x => x.ClearAuthenticationCookie());

            var service = new AggregateUserService(membership, aggregateRepository, formsAuthService, null);

            // Act
            var result = service.Authenticate("admin", "123", true, new List<UserRole> { UserRole.Admin });

            // Assert
            membership.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            Assert.IsFalse(result);
        }

        [Test]
        public void Valid_Credentials_With_The_Right_UserRole_Returns_Good_Execution_State()
        {
            // Arrange
            var membershipUser = new PfMembershipUser() { UserName = "12345678" };

            var membership = MockRepository.GenerateMock<IMembershipService>();
            membership.Expect(x => x.ValidateUserByEmailAddr("admin", "123")).Return(membershipUser);

            var aggrUser = new AggregateUser
            {
                IdentityProfile = new IdentityProfile { UserRole = UserRole.Admin },
                Membership = membershipUser,
            };

            var aggregateRepository = MockRepository.GenerateMock<IAggregateUserRepository>();
            aggregateRepository.Expect(x => x.RetrieveByMembershipUserName("12345678")).Return(aggrUser);

            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            formsAuthService.Expect(x => x.SetAuthCookieForUser("12345678", true));

            var service = new AggregateUserService(membership, aggregateRepository, formsAuthService, null);

            // Act
            var result = service.Authenticate("admin", "123", true, new List<UserRole> { UserRole.Admin });

            // Assert
            membership.VerifyAllExpectations();
            aggregateRepository.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            Assert.True(result);
        }



        [Test]
        public void Valid_Forms_Authenticated_User_That_Exists_In_Repository_Touches_Membership()
        {
            // Arrange
            var aggrUserRepository = MockRepository.GenerateMock<IAggregateUserRepository>();
            var membershipService = MockRepository.GenerateMock<IMembershipService>();
            var httpContext = HttpContextStubFactory.Create(AuthenticatedName: "12345678", IsAuthenticated: true);
            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();

            // ... set expectations

            var aggrUser = new AggregateUser() { Membership = new PfMembershipUser {UserName = "12345678"}};
            aggrUserRepository.Expect(x => x.RetrieveByMembershipUserName("12345678")).Return(aggrUser);            
            membershipService.Expect(x => x.Touch("12345678"));

            // Act
            var service = new AggregateUserService(membershipService, aggrUserRepository, formsAuthService, null);
            var user = service.LoadAuthentedUserIntoContext(httpContext);

            // Assert
            aggrUserRepository.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            membershipService.VerifyAllExpectations();
            Assert.AreSame(aggrUser, user);
        }

        [Test]
        public void Valid_Forms_Authenticated_User_That_DoesNOT_Exist_In_Repository_Clears_Cookies_And_Returns_Anoymous_User()
        {
            // Arrange
            var aggrUserRepository = MockRepository.GenerateMock<IAggregateUserRepository>();
            var membershipService = MockRepository.GenerateMock<IMembershipService>();
            var httpContext = HttpContextStubFactory.Create(AuthenticatedName: "12345678", IsAuthenticated: true);
            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();

            // Arrange
            var aggrUser = new AggregateUser();
            aggrUserRepository.Expect(x => x.RetrieveByMembershipUserName("12345678")).Return(null);
            formsAuthService.Expect(x => x.ClearAuthenticationCookie());

            // Act
            var service = new AggregateUserService(
                membershipService, aggrUserRepository, formsAuthService, null);
            var user = service.LoadAuthentedUserIntoContext(httpContext);

            // Assert
            aggrUserRepository.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            Assert.AreEqual(UserRole.Anonymous, user.IdentityProfile.UserRole);
        }

        [Test]
        public void Invalid_Forms_Authenticated_User_Returns_Anoymous_User()
        {
            // Arrange
            var aggrUser = new AggregateUser();
            var httpContext = HttpContextStubFactory.Create(AuthenticatedName: null, IsAuthenticated: false);

            // Act
            var service = new AggregateUserService(null, null, null, null);
            var user = service.LoadAuthentedUserIntoContext(httpContext);

            // Assert
            Assert.AreEqual(UserRole.Anonymous, user.IdentityProfile.UserRole);
        }

        [Test]
        public void User_Already_Cached_In_HttpContext_Returns_User()
        {
            // Arrange
            var aggrUser = new AggregateUser();
            var httpContext = HttpContextStubFactory.Create(AuthenticatedName: null, IsAuthenticated: false);
            httpContext.StoreAggregateUserInContext(aggrUser);

            // Act
            var service = new AggregateUserService(null, null, null, null);
            var user = service.LoadAuthentedUserIntoContext(httpContext);

            // Assert
            Assert.AreEqual(user, aggrUser);
        }

        // TOOD: more tests for LoadAuthentedUserIntoContext (???)



        [Test]
        [ExpectedException(typeof(Exception))]
        public void Cant_Add_More_Than_Maximum_Number_Of_Admins()
        {
            // Arrange
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            var aggregateUserService = new  AggregateUserService(null, repository, null, null);
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
            var aggregateUserService = new AggregateUserService(null, repository, null, null);
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
            var aggregateUserService = new AggregateUserService(null, null, null, null);

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

            var aggregateUserService = new AggregateUserService(membership, null, null, null);

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
            repository.Expect(x => x.Insert(null)).IgnoreArguments();

            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            unitOfWork.Expect(x => x.SaveChanges());

            PleiadesMembershipCreateStatus createStatus;
            var membership = MockRepository.GenerateMock<IMembershipService>();
            membership.Expect(x => x.CreateUser(null, out createStatus)).IgnoreArguments().OutRef(PleiadesMembershipCreateStatus.Success);

            // Act
            var aggregateUserService = new AggregateUserService(membership, repository, null, unitOfWork);

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

        [Test]
        public void Authorized_Users_Calling_UpdateIdentity_Suceeds()
        {
            // Arrange
            var user = new AggregateUser()
            {
                ID = 888,
                IdentityProfile = new IdentityProfile() { UserRole = UserRole.Admin, },
                Membership = new PfMembershipUser { UserName = "12345678", },
            };

            var modifyRequest = new CreateOrModifyIdentityRequest() 
                    { Id = 888, FirstName = "Jospeh", LastName = "Lambert", Email = "abc@efe.com", IsApproved = true };

            var aggrUserRepository = MockRepository.GenerateMock<IAggregateUserRepository>();
            aggrUserRepository.Expect(x => x.RetrieveById(888)).Return(user);
            aggrUserRepository.Expect(x => x.UpdateIdentity(modifyRequest));

            var membershipService = MockRepository.GenerateMock<IMembershipService>();
            membershipService.Expect(x => x.SetUserApproval("123456", true));
            membershipService.Expect(x => x.ChangeEmailAddress("123456", "abc@efe.com"));

            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            unitOfWork.Expect(x => x.SaveChanges());

            var aggregateService =
                new AggregateUserService(membershipService, aggrUserRepository, null, unitOfWork);

            // Act
            aggregateService.UpdateIdentity(modifyRequest);

            // Assert
            aggrUserRepository.VerifyAllExpectations();
            unitOfWork.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException()]
        public void Unauthorized_Owners_Calling_UpdateIdentity_Fails()
        {
            var aggregateService = new AggregateUserService(null, null, null, null);

            // Act
            aggregateService.UpdateIdentity(new CreateOrModifyIdentityRequest() 
                    { Id = 888, FirstName = "Jospeh", LastName = "Lambert", Email = "abc@efe.com", IsApproved = true });

            // Assert
            // ???
        }

        [Test]
        public void Authorized_Users_Calling_Update_Password_Succeeds()
        {
            // Arrange
            var user = new AggregateUser()
            {
                IdentityProfile = new IdentityProfile(),
                Membership = new PfMembershipUser { UserName = "12345678", }
            };

            var membershipService = MockRepository.GenerateMock<IMembershipService>();
            membershipService.Expect(x => x.ChangePassword("12345678", "12345678", "abcdef"));
            
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository.Expect(x => x.RetrieveById(888)).Return(user);

            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            unitOfWork.Expect(x => x.SaveChanges());

            var service = new AggregateUserService(membershipService, repository, null, unitOfWork);

            // Act
            service.ChangeUserPassword(888, "12345678", "abcdef");

            // Assert
            repository.VerifyAllExpectations();
            membershipService.VerifyAllExpectations();
            unitOfWork.VerifyAllExpectations();
        }
    }
}