using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Framework.Injection;
using Pleiades.Framework.TestHelpers;
using Pleiades.Framework.TestHelpers.Web;
using Pleiades.Commerce.Web.Security.Execution.Steps;
using Pleiades.Commerce.Web.Security.Model;
using Pleiades.Commerce.WebUI;
using Pleiades.Commerce.WebUI.Areas.Admin.Controllers;
using Pleiades.Commerce.WebUI.Areas.Admin.Models;

namespace Pleiades.Commerce.WebUI.TestsControllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public void TestLogonInvalidFormReturnsView()
        {
            // Arrange
            var step = MockRepository.GenerateMock<AuthenticateUserByRoleStep>();
            step.Expect(x => 
                x.Execute(null)).IgnoreArguments().Return(new AuthenticateUserByRoleContext { IsExecutionStateValid = false });

            var container = MockRepository.GenerateMock<IGenericContainer>();
            container.Expect(x => x.Resolve<AuthenticateUserByRoleStep>()).Return(step);

            var controller = new LoginController(container);

            // Act
            var result = controller.Logon(null, "");

            // Assert
            result.ShouldBeView();
        }

        //[Test]
        //public void TestLogonBadUserDataReturnsView()
        //{
        //    // Arrange
        //    var acctController = new LoginController();
        //    acctController.MembershipService = MockRepository.GenerateMock<IMembershipService>();
        //    acctController.MembershipService.Expect(x => x.ValidateUserByEmailAddr("admin", "123")).Return(null);
        //    acctController.FormsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();
        //    acctController.FormsAuthService.Expect(x => x.ClearAuthenticationCookie());

        //    // Act
        //    var result = acctController.Logon(new LogOnViewModel { UserName = "admin", Password = "123" }, null);
                
        //    // Assert
        //    result.ShouldBeView();
        //    acctController.MembershipService.VerifyAllExpectations();
        //    acctController.FormsAuthService.VerifyAllExpectations();
        //}

        //[Test]
        //public void TestLogonGoodUserNoREdirectBack()
        //{
        //    // Arrange
        //    var acctController = new LoginController();
        //    var domainUser = new DomainUser();
        //    acctController.MembershipService = MockRepository.GenerateMock<IMembershipService>();
        //    acctController.MembershipService.Expect(x => x.ValidateUserByEmailAddr("admin", "123")).Return(domainUser);
        //    acctController.FormsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();
        //    acctController.FormsAuthService.Expect(x => x.SetAuthCookieForUser(domainUser, acctController.PersistentCookie));

        //    // Act
        //    var result = acctController.Logon(new LogOnViewModel { UserName = "admin", Password = "123" }, null);

        //    // Assert
        //    result.ShouldBeRedirectionTo(new { area = "Admin", controller = "Home", action = "Index" });
        //    acctController.FormsAuthService.VerifyAllExpectations();
        //    acctController.MembershipService.VerifyAllExpectations();
        //}

        //[Test]
        //public void TestLogonGoodUserWithRedirect()
        //{
        //    // Arrange
        //    var acctController = new LoginController();
        //    var domainUser = new DomainUser();
        //    acctController.MembershipService = MockRepository.GenerateMock<IMembershipService>();
        //    acctController.MembershipService.Expect(x => x.ValidateUserByEmailAddr("admin", "123")).Return(domainUser);
        //    acctController.FormsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();
        //    acctController.FormsAuthService.Expect(x => x.SetAuthCookieForUser(domainUser, acctController.PersistentCookie));

        //    // Act
        //    var result = acctController.Logon(new LogOnViewModel { UserName = "admin", Password = "123" }, "winter/solstice/3");

        //    // Assert
        //    Assert.IsInstanceOfType<RedirectResult>(result);
        //    ((RedirectResult)result).Url.ShouldEqual("winter/solstice/3");
        //    acctController.FormsAuthService.VerifyAllExpectations();
        //    acctController.MembershipService.VerifyAllExpectations();
        //}
    }
}
