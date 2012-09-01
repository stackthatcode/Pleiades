using System.Security.Principal;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;
using Pleiades.TestHelpers.Web;

namespace Pleiades.UnitTests.Web.Security
{
    [TestFixture]
    public class GetUserFromHttpContextStepTest
    {
        [Test]
        public void Valid_Forms_Authenticated_User_That_Exists_Repository_Touches_Membership()
        {
            // Arrange
            var aggrUser = new AggregateUser();
            var aggrUserRepository = MockRepository.GenerateMock<IAggregateUserRepository>();
            aggrUserRepository.Expect(x => x.RetrieveByMembershipUserName("12345678")).Return(aggrUser);

            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();

            var membershipService = MockRepository.GenerateMock<IMembershipService>();
            membershipService.Expect(x => x.Touch("12345678"));

            var step = new GetUserFromContextStep(aggrUserRepository, formsAuthService, membershipService);
            var httpContext = HttpContextStubFactory.Make(AuthenticatedName: "12345678", IsAuthenticated: true);

            var context = new SystemAuthorizationContext(httpContext);

            // Act
            step.Execute(context);

            // Assert
            aggrUserRepository.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            membershipService.VerifyAllExpectations();

            Assert.AreSame(aggrUser, context.ThisUser);
        }

        [Test]
        public void Valid_Forms_Authenticated_User_That_DoesNOT_Exist_In_Repository_Clears_Cookies_And_Returns_Anoymous_User()
        {
            // Arrange
            var aggrUser = new AggregateUser();
            var aggrUserRepository = MockRepository.GenerateMock<IAggregateUserRepository>();
            aggrUserRepository.Expect(x => x.RetrieveByMembershipUserName("12345678")).Return(null);

            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            formsAuthService.Expect(x => x.ClearAuthenticationCookie());

            var membershipService = MockRepository.GenerateMock<IMembershipService>();

            var step = new GetUserFromContextStep(aggrUserRepository, formsAuthService, membershipService);
            var httpContext = HttpContextStubFactory.Make(AuthenticatedName: "12345678", IsAuthenticated: true);

            var context = new SystemAuthorizationContext(httpContext);

            // Act
            step.Execute(context);

            // Assert
            aggrUserRepository.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            membershipService.VerifyAllExpectations();

            Assert.AreEqual(UserRole.Anonymous, context.ThisUser.Identity.UserRole);
        }

        [Test]
        public void Invalid_Forms_Authenticated_User_Returns_Anoymous_User()
        {
            // Arrange
            var aggrUser = new AggregateUser();
            var aggrUserRepository = MockRepository.GenerateMock<IAggregateUserRepository>();
            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            var membershipService = MockRepository.GenerateMock<IMembershipService>();

            var step = new GetUserFromContextStep(aggrUserRepository, formsAuthService, membershipService);
            var httpContext = HttpContextStubFactory.Make(AuthenticatedName: null, IsAuthenticated: false);

            var context = new SystemAuthorizationContext(httpContext);

            // Act
            step.Execute(context);

            // Assert
            aggrUserRepository.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            membershipService.VerifyAllExpectations();

            Assert.AreEqual(UserRole.Anonymous, context.ThisUser.Identity.UserRole);
        }
    }
}