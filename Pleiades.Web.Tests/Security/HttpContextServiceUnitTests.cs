using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Gallio.Framework;
using MbUnit.Framework;
using Rhino.Mocks;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Model;
using Pleiades.Framework.Web.Security.Concrete;
using Pleiades.Framework.TestHelpers;
using Pleiades.Framework.TestHelpers.Web;

namespace Pleiades.Web.Tests.SecurityUnitTests
{
    [TestFixture]
    public class AuthenticationUnitTests
    {
        // QUESTION: DOES THE SQL PROVIDER RUN WHEN THE REQEUST IS SENT THROUGHT THE PIPELINE...???

        [Test]
        public void GetFormAuthenticationUserNameFromMockHttpContext()
        {
            // Configure the stub
            var context = HttpContextStubFactory.Make(
                IsAuthenticated: true, AuthenticatedName: "Admin", AuthenticationType: "Forms");
            
            // View values injected into the HttpContext stub
            IIdentity user = context.User.Identity;
            var test = user.IsAuthenticated + " " + user.Name + " " + user.AuthenticationType;
            TestLog.WriteLine(test);

            // Use the Authentication Service to extract the User Name
            var service = new HttpContextUserService();
            Assert.AreEqual("Admin", service.GetUserNameFromContext(context));

            // Playing around with mocks/stubs!
            context.Response.StatusCode = 500;            
            context.Response.StatusCode.ShouldEqual(500);
        }

        [Test]
        public void TestRetrieveUserFromHttpContextWithNull()
        {
            // Arrange
            var context = HttpContextStubFactory.Make(IsAuthenticated: true, AuthenticatedName: null);
            var service = new HttpContextUserService();

            // Act
            var user = service.RetrieveUserFromHttpContext(context);
            
            // Assert
            Assert.AreEqual(UserRole.Anonymous, user.UserRole);
        }

        [Test]
        public void TestRetrieveUserFromHttpContextWithNonExistentUserAndVerifyCookieCleared()
        {
            // Arrange
            var domainService = MockRepository.GenerateMock<IDomainUserService>();
            domainService.Expect(x => x.RetrieveUserByMembershipUserName("bob123")).Return(null);

            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            formsAuthService.Expect(x => x.ClearAuthenticationCookie());

            var context = HttpContextStubFactory.Make(IsAuthenticated: true, AuthenticatedName: "bob123");
            var service = new HttpContextUserService();
            service.DomainUserService = domainService;
            service.FormsAuthenticationService = formsAuthService;

            // Act
            var user = service.RetrieveUserFromHttpContext(context);

            // Assert
            Assert.AreEqual(UserRole.Anonymous, user.UserRole);
            domainService.VerifyAllExpectations();
            formsAuthService.VerifyAllExpectations();
        }


        [Test]
        public void TestRetrieveLegitimateUserFromHttpContext()
        {
            // Arrange
            var user = new DomainUser { UserRole = UserRole.Trusted };

            var domainService = MockRepository.GenerateMock<IDomainUserService>();
            domainService.Expect(x => 
                x.RetrieveUserByMembershipUserName("bob123"))
                .Return(user);

            var membershipService = MockRepository.GenerateMock<IMembershipAdapter>();
            membershipService.Expect(x => x.Touch(user));

            var context = HttpContextStubFactory.Make(IsAuthenticated: true, AuthenticatedName: "bob123");
            var service = new HttpContextUserService();
            service.DomainUserService = domainService;
            service.MembershipUserService = membershipService;

            // Act
            var userActual = service.RetrieveUserFromHttpContext(context);

            // Assert
            Assert.AreEqual(UserRole.Trusted, userActual.UserRole);
            domainService.VerifyAllExpectations();
            membershipService.VerifyAllExpectations();
        }

    }
}
