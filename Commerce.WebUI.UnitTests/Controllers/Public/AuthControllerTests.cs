using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Injection;
using Pleiades.TestHelpers;
using Pleiades.TestHelpers.Web;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Commerce.WebUI;
using Commerce.WebUI.Areas.Admin.Controllers;
using Commerce.WebUI.Areas.Admin.Models;
using Commerce.WebUI.Plumbing.Navigation;

namespace Commerce.WebUI.TestsControllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        [Test]
        public void Logon_Get_Returns_Default_View()
        {
            var controller = new AuthController(null, null);
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
                .Expect(x => x.Authenticate("admin", "123", AuthController.PersistentCookie, null))
                .Return(false)
                .IgnoreArguments();

            var controller = new AuthController(service, null);

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
            var service = MockRepository.GenerateMock<IAggregateUserService>();
            service
                .Expect(x => x.Authenticate("admin", "123", AuthController.PersistentCookie, null))
                .Return(true)
                .IgnoreArguments();

            var controller = new AuthController(service, null);

            // Act
            var result = controller.Login(model, null);

            // Assert
            service.VerifyAllExpectations();
            result.ShouldBeRedirectionTo(OutboundNavigation.AdminHome());
        }

        [Test]
        public void Logon_Good_Credentials_With_NonNull_ReturnUrl_Returns_Redirect_To_Admin_Home()
        {
            // Arrange
            var model = new LogOnViewModel { UserName = "admin", Password = "123" };
            var service = MockRepository.GenerateMock<IAggregateUserService>();
            service
                .Expect(x => x.Authenticate("admin", "123", AuthController.PersistentCookie, null))
                .Return(true)
                .IgnoreArguments();

            var controller = new AuthController(service, null);

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

            var controller = new AuthController(null, service);

            // Act
            var result = controller.Logout();
            
            // Assert
            service.VerifyAllExpectations();
            result.ShouldBeRedirectionTo(OutboundNavigation.PublicHome());
        }
    }
}