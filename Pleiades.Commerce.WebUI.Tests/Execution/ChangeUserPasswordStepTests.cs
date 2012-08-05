using System;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Commerce.Domain.Model.Users;
using Pleiades.Commerce.Web.Security.Execution.Steps;
using Pleiades.Commerce.Web.Security.Model;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Framework.MembershipProvider.Model;

namespace Pleiades.Commerce.Web.UnitTests.Execution
{
    [TestFixture]
    public class ChangeUserPasswordStepTests
    {
        [Test]
        public void Verify_Step_Properly_Invokes_Services()
        {
            // Arrange
            var user = new AggregateUser()
            {
                IdentityUser = new IdentityUser(),
                MembershipUserName = "12345678",
            };

            var membershipService = MockRepository.GenerateMock<IMembershipService>();
            membershipService.Expect(x => x.ChangePassword("12345678", "123456", "abcdef"));

            var step = new ChangeUserPasswordStep(membershipService);
            var context = new ChangeUserPasswordContext()
            {
                 ThisUser = user,
                 OwnerUser = user,
                 OldPassword = "123456",
                 NewPassword = "abcdef",
            };

            // Act
            step.Execute(context);

            // Assert
            membershipService.VerifyAllExpectations();
        }
    }
}