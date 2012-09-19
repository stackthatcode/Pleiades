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
        [Test]
        public void Verify_Approved_Owners_Calling_UpdateIdentity_Successfully_Invokes_Services()
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
        public void Verify_Unapproved_Owners_Calling_UpdateIdentity_Throws()
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
        public void Verify_Update_Email_Method_Pass()
        {
            // Arrange
            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityResponseCode.Allowed);

            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository
                .Expect(x => x.RetrieveById(888))
                .Return(new AggregateUser { Membership = new MembershipUser { UserName = "123456" } });

            var memebrship = MockRepository.GenerateMock<IMembershipService>();
            memebrship.Expect(x => x.ChangeEmailAddress("123456", "john@john.com"));

            // Act
            var aggregateService = new AggregateUserService(memebrship, repository, ownerAuthorizationService, null);
            aggregateService.UpdateEmail(888, "john@john.com");

            // Assert
            ownerAuthorizationService.VerifyAllExpectations();
            repository.VerifyAllExpectations();
            memebrship.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException()]
        public void Verify_Update_Email_Method_Throws_For_Unapproved_Users()
        {
            // Arrange
            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityResponseCode.AccessDenied);

            // Act
            var aggregateService = new AggregateUserService(null, null, ownerAuthorizationService, null);
            aggregateService.UpdateEmail(888, "john@john.com");

            // Assert
        }

        [Test]
        [ExpectedException()]
        public void Verify_Update_Email_Method_Throws_For_Null_Email_Address()
        {
            // Arrange
            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityResponseCode.AccessDenied);

            // Act
            var aggregateService = new AggregateUserService(null, null, ownerAuthorizationService, null);
            aggregateService.UpdateEmail(888, null);
            
            // Assert
        }

        [Test]
        public void Verify_UpdateApproval_HappyPath()
        {
            // Arrange
            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityResponseCode.Allowed);

            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository
                .Expect(x => x.RetrieveById(888))
                .Return(new AggregateUser
                        {
                            Membership = new MembershipUser { UserName = "123456" },
                            IdentityProfile = new IdentityProfile { UserRole = UserRole.Trusted },
                        });

            var memebrship = MockRepository.GenerateMock<IMembershipService>();
            memebrship.Expect(x => x.SetUserApproval("123456", true));

            // Act
            var aggregateService = new AggregateUserService(memebrship, repository, ownerAuthorizationService, null);
            aggregateService.UpdateApproval(888, true);

            // Assert
            ownerAuthorizationService.VerifyAllExpectations();
            repository.VerifyAllExpectations();
            memebrship.VerifyAllExpectations();
        }

        [Test]
        public void Verify_UpdateApproval_Cannot_Update_SupremeUser()
        {
            // Arrange
            var ownerAuthorizationService = MockRepository.GenerateMock<IOwnerAuthorizationService>();
            ownerAuthorizationService.Expect(x => x.Authorize(888)).Return(SecurityResponseCode.Allowed);

            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository
                .Expect(x => x.RetrieveById(888))
                .Return(new AggregateUser
                {
                    Membership = new MembershipUser { UserName = "123456" },
                    IdentityProfile = new IdentityProfile { UserRole = UserRole.Supreme },
                });

            // Act
            var aggregateService = new AggregateUserService(null, repository, ownerAuthorizationService, null);
            aggregateService.UpdateApproval(888, true);

            // Assert
            ownerAuthorizationService.VerifyAllExpectations();
            repository.VerifyAllExpectations();
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
    }
}