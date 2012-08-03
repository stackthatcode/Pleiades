using System.Security.Principal;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Commerce.Domain.Model.Users;
using Pleiades.Commerce.Domain.Interface;
using Pleiades.Commerce.Web.Security.Execution;
using Pleiades.Commerce.Web.Security.Model;
using Pleiades.Framework.Identity.Interface;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Framework.TestHelpers.Web;

namespace Pleiades.Commerce.Web.UnitTests.Execution
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
            aggrUserRepository.Expect(x => x.RetrieveUserByMembershipUserName("12345678")).Return(aggrUser);

            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();

            var membershipService = MockRepository.GenerateMock<IMembershipService>();
            membershipService.Expect(x => x.Touch("12345678"));

            var step = new GetUserFromContextStep(aggrUserRepository, formsAuthService, membershipService);
            var httpContext = HttpContextStubFactory.Make(AuthenticatedName: "12345678", IsAuthenticated: true);

            var context = new SystemAuthorizationContextBase()
            {
                HttpContext = httpContext,
            };

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
            aggrUserRepository.Expect(x => x.RetrieveUserByMembershipUserName("12345678")).Return(null);

            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            formsAuthService.Expect(x => x.ClearAuthenticationCookie());

            var membershipService = MockRepository.GenerateMock<IMembershipService>();

            var step = new GetUserFromContextStep(aggrUserRepository, formsAuthService, membershipService);
            var httpContext = HttpContextStubFactory.Make(AuthenticatedName: "12345678", IsAuthenticated: true);

            var context = new SystemAuthorizationContextBase()
            {
                HttpContext = httpContext,
            };

            // Act
            step.Execute(context);

            // Assert
            aggrUserRepository.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            membershipService.VerifyAllExpectations();

            Assert.AreEqual(UserRole.Anonymous, context.ThisUser.IdentityUser.UserRole);
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

            var context = new SystemAuthorizationContextBase()
            {
                HttpContext = httpContext,
            };

            // Act
            step.Execute(context);

            // Assert
            aggrUserRepository.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
            membershipService.VerifyAllExpectations();

            Assert.AreEqual(UserRole.Anonymous, context.ThisUser.IdentityUser.UserRole);
        }
    }
}