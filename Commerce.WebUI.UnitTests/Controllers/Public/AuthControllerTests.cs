using System.Web.Mvc;
using System.Web.Routing;
using Commerce.Web;
using Commerce.Web.Controllers;
using NUnit.Framework;
using Pleiades.TestHelpers.Web;
using Pleiades.Web.Security.Model;
using Rhino.Mocks;
using Pleiades.Web.Security.Interface;
using Commerce.Web.Areas.Admin.Models;

namespace Commerce.UnitTests.Controllers.Public
{
    [TestFixture]
    public class AuthControllerTests
    {
        [Test]
        public void Logon_Get_Returns_Default_View()
        {
            var controller = new UnsecuredController(null, null);
            var result = controller.Login();
            result.ShouldBeDefaultView();
        }

        [Test]
        public void Logon_Bad_Credentials_Returns_Same_View()
        {
            // Arrange
            var model = new LogOnViewModel { UserName = "admin", Password = "123" };
            var service = MockRepository.GenerateMock<IAggregateUserService>();
            service
                .Expect(x => x.Authenticate("admin", "123", UnsecuredController.PersistentCookie, null))
                .Return(null)
                .IgnoreArguments();

            var controller = new UnsecuredController(service, null);

            // Act
            var result = controller.Login(model, null);

            // Assert
            service.VerifyAllExpectations();
            result.ShouldBeDefaultView();
        }

        [Test]
        public void Logon_Good_Credentials_With_Null_ReturnUrl_Returns_Redirect_To_Admin_Home()
        {
            // Arrange
            var model = new LogOnViewModel { UserName = "admin", Password = "123" };
            var user = new AggregateUser {};
            var service = MockRepository.GenerateMock<IAggregateUserService>();
            service
                .Expect(x => x.Authenticate("admin", "123", UnsecuredController.PersistentCookie, null))
                .IgnoreArguments()
                .Return(user);

            var controller = new UnsecuredController(service, null);

            // Act
            var result = controller.Login(model, null);

            // Assert
            service.VerifyAllExpectations();
            result.ShouldBeRedirectionTo(AdminNavigation.Home());
        }

        [Test]
        public void Logon_Good_Credentials_With_NonNull_ReturnUrl_Returns_Redirect_To_Admin_Home()
        {
            // Arrange
            var model = new LogOnViewModel { UserName = "admin", Password = "123" };
            var user = new AggregateUser();
            var service = MockRepository.GenerateMock<IAggregateUserService>();
            service
                .Expect(x => x.Authenticate("admin", "123", UnsecuredController.PersistentCookie, null))
                .Return(user)
                .IgnoreArguments();

            var controller = new UnsecuredController(service, null);
            var context = HttpContextStubFactory.Create();
            context.Server.Expect(x => x.UrlDecode("http://google.com")).Return("MyUrl.aspx").IgnoreArguments();

            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);

            // Act
            var result = controller.Login(model, "MyUrl.aspx");

            // Assert
            service.VerifyAllExpectations();
            result.ShouldBeRedirectionTo("MyUrl.aspx");
        }

        [Test]
        public void Logout_Executes_FormsAuthService_And_Returns_Redirect_To_Public_Home()
        {
            // Arrange
            var model = new LogOnViewModel { UserName = "admin", Password = "123" };
            var service = MockRepository.GenerateMock<IFormsAuthenticationService>();
            service.Expect(x => x.ClearAuthenticationCookie());

            var controller = new UnsecuredController(null, service);

            // Act
            var result = controller.Logout();
            
            // Assert
            service.VerifyAllExpectations();
            result.ShouldBeRedirectionTo(AdminNavigation.Login());
        }
    }
}