using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.TestHelpers.Web;
using Pleiades.Web.Security.Utility;
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
            var membership = MockRepository.GenerateMock<IPfMembershipService>();
            membership.Expect(x => x.ValidateUserByEmailAddr("admin", "123")).Return(null);

            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            formsAuthService.Expect(x => x.ClearAuthenticationCookie());

            var service = new AggregateUserService(membership, null, null, formsAuthService);

            // Act
            var result = service.Authenticate("admin", "123", true, null);

            // Assert
            membership.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Valid_Credentials_With_The_Wrong_UserRole_Returns_Bad_Execution_State_And_Clears_Cookie()
        {
            // Arrange
            var membershipUser = new PfMembershipUser() { UserName = "12345678" };

            var membership = MockRepository.GenerateMock<IPfMembershipService>();
            membership.Expect(x => x.ValidateUserByEmailAddr("admin", "123")).Return(membershipUser);

            var aggrUser = new AggregateUser
            {
                IdentityProfile = new IdentityProfile { UserRole = UserRole.Trusted },
                Membership = membershipUser,
            };

            var aggregateRepository = MockRepository.GenerateMock<IReadOnlyAggregateUserRepository>();
            aggregateRepository.Expect(x => x.RetrieveByMembershipUserName("12345678")).Return(aggrUser);

            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            formsAuthService.Expect(x => x.ClearAuthenticationCookie());

            var service = new AggregateUserService(membership, aggregateRepository, null, formsAuthService);

            // Act
            var result = service.Authenticate("admin", "123", true, new List<UserRole> { UserRole.Admin });

            // Assert
            membership.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Valid_Credentials_With_The_Right_UserRole_Returns_Good_Execution_State()
        {
            // Arrange
            var membershipUser = new PfMembershipUser() { UserName = "12345678" };

            var membership = MockRepository.GenerateMock<IPfMembershipService>();
            membership.Expect(x => x.ValidateUserByEmailAddr("admin", "123")).Return(membershipUser);

            var aggrUser = new AggregateUser
            {
                IdentityProfile = new IdentityProfile { UserRole = UserRole.Admin },
                Membership = membershipUser,
            };

            var aggregateRepository = MockRepository.GenerateMock<IReadOnlyAggregateUserRepository>();
            aggregateRepository.Expect(x => x.RetrieveByMembershipUserName("12345678")).Return(aggrUser);

            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            formsAuthService.Expect(x => x.SetAuthCookieForUser("12345678", true));

            var service = new AggregateUserService(membership, aggregateRepository, null, formsAuthService);

            // Act
            var result = service.Authenticate("admin", "123", true, new List<UserRole> { UserRole.Admin });

            // Assert
            membership.VerifyAllExpectations();
            aggregateRepository.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            Assert.That(result, Is.Not.Null);
        }



        [Test]
        public void Invalid_Forms_Authenticated_User_Returns_Anoymous_User()
        {
            // Arrange
            var httpContext = HttpContextStubFactory.Create(AuthenticatedName: null, IsAuthenticated: false);

            // Act
            var service = new AggregateUserService(null, null, null, null);
            var user = service.LoadAuthentedUserIntoContext(httpContext);

            // Assert
            Assert.AreEqual(UserRole.Anonymous, user.IdentityProfile.UserRole);
        }

        [Test]
        public void Valid_Forms_Authenticated_User_That_Exists_In_Repository_Touches_Membership()
        {
            // Arrange
            var aggrUserRepository = MockRepository.GenerateMock<IReadOnlyAggregateUserRepository>();
            var membershipService = MockRepository.GenerateMock<IPfMembershipService>();
            var httpContext = HttpContextStubFactory.Create(AuthenticatedName: "12345678", IsAuthenticated: true);
            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();

            // ... set expectations

            var aggrUser = new AggregateUser() { Membership = new PfMembershipUser {UserName = "12345678"}};
            aggrUserRepository.Expect(x => x.RetrieveByMembershipUserName("12345678")).Return(aggrUser);            
            membershipService.Expect(x => x.Touch("12345678"));

            // Act
            var service = new AggregateUserService(membershipService, aggrUserRepository, null, formsAuthService);
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
            var aggrUserRepository = MockRepository.GenerateMock<IReadOnlyAggregateUserRepository>();
            var membershipService = MockRepository.GenerateMock<IPfMembershipService>();
            var httpContext = HttpContextStubFactory.Create(AuthenticatedName: "12345678", IsAuthenticated: true);
            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();

            // Arrange
            var aggrUser = new AggregateUser();
            aggrUserRepository.Expect(x => x.RetrieveByMembershipUserName("12345678")).Return(null);
            formsAuthService.Expect(x => x.ClearAuthenticationCookie());

            // Act
            var service = new AggregateUserService(membershipService, aggrUserRepository, null, formsAuthService);
            var user = service.LoadAuthentedUserIntoContext(httpContext);

            // Assert
            aggrUserRepository.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
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
        public void Cant_Add_More_Than_Maximum_Number_Of_Admins()
        {
            // Arrange
            var repository = MockRepository.GenerateMock<IReadOnlyAggregateUserRepository>();
            var aggregateUserService = new  AggregateUserService(null, repository, null, null);
            repository.Expect(x => x.GetUserCountByRole(UserRole.Admin)).Return(AggregateUserService.MaxAdminUsers);

            // Act
            string message;
            var result = aggregateUserService.Create(null, new IdentityProfileChange() { UserRole = UserRole.Admin }, out message);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Cant_Add_More_Than_Maximum_Number_Of_Root_Users()
        {
            // Arrange
            var repository = MockRepository.GenerateMock<IReadOnlyAggregateUserRepository>();
            var aggregateUserService = new AggregateUserService(null, repository, null, null);
            repository.Expect(x => x.GetUserCountByRole(UserRole.Admin)).Return(1);
            repository.Expect(x => x.GetUserCountByRole(UserRole.Root)).Return(AggregateUserService.MaxRootUsers);

            // Act
            string message;
            var result = aggregateUserService.Create(null, new IdentityProfileChange() { UserRole = UserRole.Root }, out message);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Cant_Add_Anonymous_User()
        {
            // Arrange            
            var request = new IdentityProfileChange()
            {
                UserRole = UserRole.Anonymous,
                AccountLevel = AccountLevel.Gold,
                AccountStatus = AccountStatus.Active,
                FirstName = "Jon",
                LastName = "Smith",
            };
            var aggregateUserService = new AggregateUserService(null, null, null, null);

            // Act
            string message;
            var result = aggregateUserService.Create(null, request, out message);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void MembershipFailure_Create_Request()
        {
            // Arrange            
            var request = new IdentityProfileChange()
            {
                UserRole = UserRole.Trusted,
                AccountLevel = AccountLevel.Gold,
                AccountStatus = AccountStatus.Active,
                FirstName = "Jon",
                LastName = "Smith",
            };

            // Arrange
            var membershipCreatePayload = new PfCreateNewMembershipUserRequest();

            var membership = MockRepository.GenerateMock<IPfMembershipService>();
            PfMembershipCreateStatus createStatus;
            membership.Expect(x => x.CreateUser(membershipCreatePayload, out createStatus))
                .IgnoreArguments()
                .OutRef(PfMembershipCreateStatus.DuplicateUserName);

            var aggregateUserService = new AggregateUserService(membership, null, null, null);

            // Act
            string message;
            var response = aggregateUserService.Create(null, new IdentityProfileChange() { UserRole = UserRole.Trusted }, out message);

            // Assert
            membership.VerifyAllExpectations();
            Assert.IsNull(response);
        }

        [Test]
        public void Valid_Create_Request_Passes()
        {
            // Arrange            
            var request = new IdentityProfileChange()
            {
                UserRole = UserRole.Trusted,
                AccountLevel = AccountLevel.Gold,
                AccountStatus = AccountStatus.Active,
                FirstName = "Jon",
                LastName = "Smith",
            };

            // Arrange
            var repository = MockRepository.GenerateMock<IWritableAggregateUserRepository>();
            repository.Expect(x => x.Add(null)).IgnoreArguments();

            var mesage = "";
            PfMembershipCreateStatus createStatus;
            var membership = MockRepository.GenerateMock<IPfMembershipService>();
            membership
                .Expect(x => x.CreateUser(null, out createStatus))
                .IgnoreArguments()
                .OutRef(PfMembershipCreateStatus.Success);

            // Act
            var aggregateUserService = new AggregateUserService(membership, null, repository, null);

            var response = aggregateUserService.Create(null,
                    new IdentityProfileChange() 
                    {
                        AccountStatus = AccountStatus.Active, AccountLevel = AccountLevel.Gold, UserRole = UserRole.Trusted 
                    },
                    out mesage);

            // Assert
            repository.VerifyAllExpectations();
            membership.VerifyAllExpectations();
            Assert.IsNotNull(response);
        }

        [Test]
        public void Authorized_Users_Calling_UpdateIdentity_Suceeds()
        {
            // Arrange
            var user = new AggregateUser()
            {
                ID = 888,
                IdentityProfile = new IdentityProfile()
                    {
                        UserRole = UserRole.Trusted, AccountLevel = AccountLevel.Standard, AccountStatus = AccountStatus.Disabled
                    },
                Membership = new PfMembershipUser { UserName = "12345678", },
            };

            var modifyRequest = new IdentityProfileChange()
                {
                    FirstName = "Jospeh", LastName = "Lambert", AccountLevel = AccountLevel.Gold, AccountStatus = AccountStatus.Active
                };
            var aggrUserRepository = MockRepository.GenerateMock<IWritableAggregateUserRepository>();
            aggrUserRepository.Expect(x => x.RetrieveById(888)).Return(user);
            var aggregateService = new AggregateUserService(null, null, aggrUserRepository, null);

            // Act
            aggregateService.UpdateIdentity(888, modifyRequest);

            // Assert
            aggrUserRepository.VerifyAllExpectations();
            Assert.That(user.IdentityProfile.FirstName, Is.EqualTo("Jospeh"));
            Assert.That(user.IdentityProfile.LastName, Is.EqualTo("Lambert"));
            Assert.That(user.IdentityProfile.AccountLevel, Is.EqualTo(AccountLevel.Gold));
            Assert.That(user.IdentityProfile.AccountStatus, Is.EqualTo(AccountStatus.Active));
        }
    }
}