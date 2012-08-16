using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Framework.Execution;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Framework.Web.Security.Execution.Steps;

namespace Pleiades.Commerce.Web.UnitTests.Execution
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