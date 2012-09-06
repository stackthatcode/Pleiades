using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Execution;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Execution.Step;

namespace Pleiades.UnitTests.Web.Security.Aggregate.Step
{
    [TestFixture]
    public class LogoutStepTests
    {
        [Test]
        public void Verify_LogoutStep_Clears_Forms_Auth_Cookie()
        {
            // Arrange
            var context = new BareContext();
            var formsAuthService = MockRepository.GenerateMock<IFormsAuthenticationService>();
            formsAuthService.Expect(x => x.ClearAuthenticationCookie());
            var step = new LogoutStep(formsAuthService);

            // Act
            step.Execute(context);

            // Assert
            formsAuthService.VerifyAllExpectations();
        }
    }
}