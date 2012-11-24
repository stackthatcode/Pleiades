using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.TestHelpers.Web;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.UnitTests.Web.Security.Aggregate.Step
{
    [TestFixture]
    public class AuthenticateUserByRoleStepTests
    {
        #region Authenticate() Tests
        [Test]
        public void Invalid_Credentials_Returns_Bad_Execution_State_And_Clears_Cookie()
        {
            // Arrange
            var membership = MockRepository.GenerateMock<IMembershipService>();
            membership.Expect(x => x.ValidateUserByEmailAddr("admin", "123")).Return(null);

            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            formsAuthService.Expect(x => x.ClearAuthenticationCookie());

            var service = new AggregateUserService(membership, null, null, formsAuthService, null);
            
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
            var membershipUser = new MembershipUser() { UserName = "12345678" };

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

            var service = new AggregateUserService(membership, aggregateRepository, null, formsAuthService, null);
            
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
            var membershipUser = new MembershipUser() { UserName = "12345678" };

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

            var service = new AggregateUserService(membership, aggregateRepository, null, formsAuthService, null);

            // Act
            var result = service.Authenticate("admin", "123", true, new List<UserRole> { UserRole.Admin });

            // Assert
            membership.VerifyAllExpectations();
            aggregateRepository.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            Assert.True(result);
        }
        #endregion

        #region GetAuthenticatedUser() Tests
        [Test]
        public void Valid_Forms_Authenticated_User_That_Exists_Repository_Touches_Membership()
        {
            // Arrange
            var httpContextUserService = MockRepository.GenerateMock<IHttpContextUserService>();
            var aggrUserRepository = MockRepository.GenerateMock<IAggregateUserRepository>();
            var membershipService = MockRepository.GenerateMock<IMembershipService>();
            var httpContext = HttpContextStubFactory.Create(AuthenticatedName: "12345678", IsAuthenticated: true);
            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();

            // ... set expectations
            httpContextUserService.Expect(x => x.Get()).Return(null);
            var aggrUser = new AggregateUser();
            aggrUserRepository.Expect(x => x.RetrieveByMembershipUserName("12345678")).Return(aggrUser);
            httpContextUserService.Expect(x => x.Put(null)).IgnoreArguments();
            membershipService.Expect(x => x.Touch("12345678"));

            // Act
            var service = new AggregateUserService(membershipService, aggrUserRepository, null, formsAuthService, httpContextUserService);
            var user = service.GetAuthenticatedUser(httpContext);
            
            // Assert
            httpContextUserService.VerifyAllExpectations();
            aggrUserRepository.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            membershipService.VerifyAllExpectations();
            Assert.AreSame(aggrUser, user);
        }

        [Test]
        public void Valid_Forms_Authenticated_User_That_DoesNOT_Exist_In_Repository_Clears_Cookies_And_Returns_Anoymous_User()
        {
            // Arrange
            var httpContextUserService = MockRepository.GenerateMock<IHttpContextUserService>();
            var aggrUserRepository = MockRepository.GenerateMock<IAggregateUserRepository>();
            var membershipService = MockRepository.GenerateMock<IMembershipService>();
            var httpContext = HttpContextStubFactory.Create(AuthenticatedName: "12345678", IsAuthenticated: true);
            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();

            // Arrange
            httpContextUserService.Expect(x => x.Get()).Return(null);
            var aggrUser = new AggregateUser();
            aggrUserRepository.Expect(x => x.RetrieveByMembershipUserName("12345678")).Return(null);
            formsAuthService.Expect(x => x.ClearAuthenticationCookie());

            // Act
            var service = new AggregateUserService(membershipService, aggrUserRepository, null, formsAuthService, httpContextUserService);
            var user = service.GetAuthenticatedUser(httpContext);
            
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
            var httpContextUserService = MockRepository.GenerateMock<IHttpContextUserService>();
            httpContextUserService.Expect(x => x.Get()).Return(null);

            // Act
            var service = new AggregateUserService(null, null, null, null, httpContextUserService);
            var user = service.GetAuthenticatedUser(httpContext);

            // Assert
            Assert.AreEqual(UserRole.Anonymous, user.IdentityProfile.UserRole);
        }

        [Test]
        public void User_Already_Cached_In_HttpContext_Returns_User()
        {
            // Arrange
            var aggrUser = new AggregateUser();
            var httpContext = HttpContextStubFactory.Create(AuthenticatedName: null, IsAuthenticated: false);
            var httpContextUserService = MockRepository.GenerateMock<IHttpContextUserService>();
            httpContextUserService.Expect(x => x.Get()).Return(aggrUser);

            // Act
            var service = new AggregateUserService(null, null, null, null, httpContextUserService);
            var user = service.GetAuthenticatedUser(httpContext);

            // Assert
            Assert.AreEqual(user, aggrUser);
        }
        #endregion
    }
}