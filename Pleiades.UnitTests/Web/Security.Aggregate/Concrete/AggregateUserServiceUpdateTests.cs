using System;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Security;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;

namespace Pleiades.UnitTests.Web.Security.Aggregate.Step
{
    [TestFixture]
    public class UpdateUserStepTests
    {
        // *** TODO - Verify Update Email method *** //

        // *** TODO - Verify Change Address method *** //


        [Test]
        public void Verify_Approved_Owners_Calling_UpdateIdentify_Successfully_Invokes_Services()
        {
            // Arrange
            var user = new AggregateUser()
            {
                ID = 888,
                IdentityProfile = new IdentityProfile()
                {
                    UserRole = UserRole.Admin,
                },
                Membership = new MembershipUser
                {
                    UserName = "12345678",
                },
            };

            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityResponseCode.Allowed);

            var aggregateUserRepository = MockRepository.GenerateMock<IAggregateUserRepository>();
            aggregateUserRepository.Expect(x => x.RetrieveById(888)).Return(user);
            aggregateUserRepository.Expect(x => x.UpdateIdentity(888, null)).IgnoreArguments();

            var aggregateService = new AggregateUserService(null, aggregateUserRepository, ownerAuthorizationService, null);

            // Act
            aggregateService.UpdateIdentity(888,
                new CreateOrModifyIdentityRequest() { FirstName = "Jospeh", LastName = "Lambert" });

            // Assert
            ownerAuthorizationService.VerifyAllExpectations();
            aggregateUserRepository.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException()]
        public void Verify_Unapproved_Owners_Calling_Update_Throws()
        {
            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityResponseCode.AccessDenied);

            var aggregateService = new AggregateUserService(null, null, ownerAuthorizationService, null);

            // Act
            aggregateService.UpdateIdentity(888,
                new CreateOrModifyIdentityRequest() { FirstName = "Jospeh", LastName = "Lambert" });

            // Assert
            ownerAuthorizationService.VerifyAllExpectations();
        }

        [Test]
        public void Verify_Update_Password_Properly_Invokes_Services()
        {
            // Arrange
            var user = new AggregateUser()
            {
                IdentityProfile = new IdentityProfile(),
                Membership = new MembershipUser
                {
                    UserName = "12345678",
                }
            };

            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository.Expect(x => x.RetrieveById(888)).Return(user);

            var membershipService = MockRepository.GenerateMock<IMembershipService>();
            membershipService.Expect(x => x.ChangePassword("12345678", "12345678", "abcdef"));

            var service = new AggregateUserService(membershipService, repository, null, null);           

            // Act
            service.ChangeUserPassword(888, "12345678", "abcdef");

            // Assert
            membershipService.VerifyAllExpectations();
        }


        // *** TODO - More Unit Testing *** //

        [Test]
        [Ignore]
        public void Verify_That_Supreme_User_Cannot_Be_Unapproved()
        {
        }



        //var membershipService = MockRepository.GenerateMock<IMembershipService>();
        //membershipService.Expect(x => x.SetUserApproval("12345678", true));
        //membershipService.Expect(x => x.ChangeEmailAddress("12345678", "ajones1@ncsa.uiuc.edu"));

        //[Test]
        //[ExpectedException()]
        //public void Verify_That_Null_Email_Address_Throws()
        //{
        //    // Arrange
        //    var context = new UpdateUserContext(null, null)
        //    {
        //        NewIsApproved = true,
        //        NewEmail = null, 
        //    };

        //    var step = new UpdateUserStep(null, null);

        //    // Act
        //    step.Execute(context);

        //    // Assert - nothing: should throw Exception
        //}

    }
}