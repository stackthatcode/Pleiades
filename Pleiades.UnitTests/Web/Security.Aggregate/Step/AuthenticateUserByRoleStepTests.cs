using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Execution;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Model;

namespace Pleiades.UnitTests.Web.Security.Aggregate.Step
{
    [TestFixture]
    public class AuthenticateUserByRoleStepTests
    {
        [Test]
        public void Invalid_Credentials_Returns_Bad_Execution_State_And_Clears_Cookie()
        {
            // Arrange
            var membership = MockRepository.GenerateMock<IMembershipService>();
            membership.Expect(x => x.ValidateUserByEmailAddr("admin", "123")).Return(null);
            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            formsAuthService.Expect(x => x.ClearAuthenticationCookie());

            var step = new AuthenticateUserByRoleStep(membership, null, formsAuthService);
            var context = new AuthenticateUserByRoleContext() { AttemptedUserName = "admin", AttemptedPassword = "123" };

            // Act
            step.Execute(context);
                
            // Assert
            membership.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            Assert.IsFalse(context.IsExecutionStateValid);
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

            var step = new AuthenticateUserByRoleStep(membership, aggregateRepository, formsAuthService);
            var context = new AuthenticateUserByRoleContext() 
            { 
                AttemptedUserName = "admin", AttemptedPassword = "123", ExpectedRoles = new List<UserRole> { UserRole.Admin },
            };

            // Act
            step.Execute(context);

            // Assert
            membership.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            Assert.IsFalse(context.IsExecutionStateValid);
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

            var step = new AuthenticateUserByRoleStep(membership, aggregateRepository, formsAuthService);
            var context = new AuthenticateUserByRoleContext()
            {
                AttemptedUserName = "admin",
                AttemptedPassword = "123",
                ExpectedRoles = new List<UserRole> { UserRole.Admin },
                PersistenceCookie = true,
            };

            // Act
            step.Execute(context);

            // Assert
            membership.VerifyAllExpectations();
            aggregateRepository.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            Assert.True(context.IsExecutionStateValid);
        }

    }
}