using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Gallio.Framework;
using MbUnit.Framework;
using Rhino.Mocks;
using Pleiades.Commerce.WebUI;
using Pleiades.Commerce.WebUI.Areas.Admin.Controllers;
using Pleiades.Commerce.WebUI.Areas.Admin.Models;
using Pleiades.Utilities.TestHelpers.General;
using Pleiades.Utilities.TestHelpers.Web;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.Commerce.UnitTests.WebUI.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public void TestLogonInvalidFormReturnsView()
        {
            // Arrange
            var acctController = new AccountController();

            // Act
            acctController.ModelState.AddModelError("", "bad creds");
            var result = acctController.Logon(new LogOnViewModel(), null);

            // Assert
            result.ShouldBeView();
        }

        [Test]
        public void TestLogonBadUserDataReturnsView()
        {
            // Arrange
            var acctController = new AccountController();
            acctController.UserService = MockRepository.GenerateMock<IDomainUserService>();
            acctController.UserService.Expect(x => x.ValidateUserByEmailAddr("admin", "123")).Return(null);
            acctController.AuthService = MockRepository.GenerateMock<IHttpContextUserService>();
            acctController.AuthService.Expect(x => x.ClearAuthenticationCookie());

            // Act
            var result = acctController.Logon(new LogOnViewModel { UserName = "admin", Password = "123" }, null);
                
            // Assert
            result.ShouldBeView();
            acctController.UserService.VerifyAllExpectations();
            acctController.AuthService.VerifyAllExpectations();
        }

        [Test]
        public void TestLogonGoodUserNoREdirectBack()
        {
            // Arrange
            var acctController = new AccountController();
            var domainUser = new DomainUser();
            acctController.UserService = MockRepository.GenerateMock<IDomainUserService>();
            acctController.UserService.Expect(x => x.ValidateUserByEmailAddr("admin", "123")).Return(domainUser);
            acctController.AuthService = MockRepository.GenerateMock<IHttpContextUserService>();
            acctController.AuthService.Expect(x => x.SetAuthCookieForUser(domainUser, acctController.PersistentCookie));

            // Act
            var result = acctController.Logon(new LogOnViewModel { UserName = "admin", Password = "123" }, null);

            // Assert
            result.ShouldBeRedirectionTo(new { area = "Admin", controller = "Home", action = "Index" });
            acctController.UserService.VerifyAllExpectations();
            acctController.AuthService.VerifyAllExpectations();
        }

        [Test]
        public void TestLogonGoodUserWithRedirect()
        {
            // Arrange
            var acctController = new AccountController();
            var domainUser = new DomainUser();
            acctController.UserService = MockRepository.GenerateMock<IDomainUserService>();
            acctController.UserService.Expect(x => x.ValidateUserByEmailAddr("admin", "123")).Return(domainUser);
            acctController.AuthService = MockRepository.GenerateMock<IHttpContextUserService>();
            acctController.AuthService.Expect(x => x.SetAuthCookieForUser(domainUser, acctController.PersistentCookie));

            // Act
            var result = acctController.Logon(new LogOnViewModel { UserName = "admin", Password = "123" }, "winter/solstice/3");

            // Assert
            Assert.IsInstanceOfType<RedirectResult>(result);
            ((RedirectResult)result).Url.ShouldEqual("winter/solstice/3");
            acctController.UserService.VerifyAllExpectations();
            acctController.AuthService.VerifyAllExpectations();
        }
    }
}
