using System;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Pleiades.UnitTests.Web.Security
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
                Identity = new IdentityUser(),
                Membership = new MembershipUser
                {
                    UserName = "12345678",
                }
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