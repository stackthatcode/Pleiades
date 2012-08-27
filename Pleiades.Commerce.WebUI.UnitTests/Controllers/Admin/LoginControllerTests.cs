using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Injection;
using Pleiades.TestHelpers;
using Pleiades.TestHelpers.Web;
using Pleiades.Web.Security.Execution.Steps;
using Pleiades.Web.Security.Model;
using Commerce.WebUI;
using Commerce.WebUI.Areas.Admin.Controllers;
using Commerce.WebUI.Areas.Admin.Models;
using Commerce.WebUI.Plumbing.Navigation;

namespace Commerce.WebUI.TestsControllers
{
    [TestFixture]
    public class LoginControllerTests
    {
        [Test]
        public void Logon_Get_Returns_Default_View()
        {
            var controller = new AuthController(null);
            var result = controller.Login();
            result.ShouldBeDefaultView();
        }


        // TODO: move this and other somewhere central
        public IGenericContainer MockContainer()
        {
            return MockRepository.GenerateMock<IGenericContainer>();
        }

        // TODO: move this and other somewhere central
        public void MockContainerResolve<T>(IGenericContainer container, T output)
        {
            container.Expect(x => x.Resolve<T>()).Return(output);
        }


        // This describes the basic pattern for mocking Execution Step and verify translation of UI Models into 
        // Execution Context.
        public AuthenticateUserByRoleStep MockAuthenticateUserByRoleStep(LogOnViewModel model, bool valid)
        {
            var step = MockRepository.GenerateMock<AuthenticateUserByRoleStep>(null, null, null);
            step.Expect(x => x.Execute(Arg<AuthenticateUserByRoleContext>.Matches(
                    m => m.AttemptedUserName == model.UserName && m.AttemptedPassword == model.Password)))
                .Return(new AuthenticateUserByRoleContext { IsExecutionStateValid = valid });
            return step;
        }

        // Basic Testing pattern for passing UI Model to step, verifying that 
        // 1.) Model has translated correctly to Context
        // 2.) Service located has been invoked to create Step
        // 3.) Step has been executed
        // 4.) Controller method responds to Step Execution and/or model input with correct View
        //
        // Alternate pattern #1
        // 1.) Controller method responds to Model Validation with correct View

        [Test]
        public void Logon_Bad_Credentials_Returns_Same_View()
        {
            // Arrange
            var model = new LogOnViewModel { UserName = "admin", Password = "123" };
            var step = this.MockAuthenticateUserByRoleStep(model, false);
            var container = this.MockContainer();
            MockContainerResolve<AuthenticateUserByRoleStep>(container, step);

            var controller = new AuthController(container);

            // Act
            var result = controller.Login(model, null);

            // Assert
            container.VerifyAllExpectations();
            step.VerifyAllExpectations();
            result.ShouldBeDefaultView();
        }

        [Test]
        public void Logon_Good_Credentials_With_Null_ReturnUrl_Returns_Redirect_To_Admin_Home()
        {
            // Arrange
            var model = new LogOnViewModel { UserName = "admin", Password = "123" };
            var step = MockAuthenticateUserByRoleStep(model, true);
            var container = MockRepository.GenerateMock<IGenericContainer>();
            container.Expect(x => x.Resolve<AuthenticateUserByRoleStep>()).Return(step);
            var controller = new AuthController(container);
            
            // Act
            var result = controller.Login(model, null);

            // Assert
            container.VerifyAllExpectations();
            step.VerifyAllExpectations();
            result.ShouldBeRedirectionTo(new { area = "Admin", controller = "Home", action = "Index" });
        }

        [Test]
        public void Logon_Good_Credentials_With_NonNull_ReturnUrl_Returns_Redirect_To_Admin_Home()
        {
            // Arrange
            var model = new LogOnViewModel { UserName = "admin", Password = "123" };
            var step = MockAuthenticateUserByRoleStep(model, true);
            var container = MockRepository.GenerateMock<IGenericContainer>();
            container.Expect(x => x.Resolve<AuthenticateUserByRoleStep>()).Return(step);
            var controller = new AuthController(container);

            // Act
            var result = controller.Login(model, "MyUrl.aspx");

            // Assert
            container.VerifyAllExpectations();
            step.VerifyAllExpectations();
            result.ShouldBeRedirectionTo("MyUrl.aspx");
        }

        [Test]
        public void Logout_Executes_LogoutStep_And_Returns_Redirect_To_Public_Home()
        {
            var container = this.MockContainer();
            var step = MockRepository.GenerateMock<LogoutStep>(new object [] { null });
            step.Expect(x => x.Execute(null)).IgnoreArguments().Return(null);
            MockContainerResolve<LogoutStep>(container, step);

            var controller = new AuthController(container);
            var result = controller.Logout();
            result.ShouldBeRedirectionTo(OutboundNavigation.PublicHome());
        }
    }
}
