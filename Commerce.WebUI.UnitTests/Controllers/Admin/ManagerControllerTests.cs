using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Application.Data;
using Pleiades.TestHelpers.Web;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Commerce.Web.Areas.Admin.Controllers;
using Commerce.Web.Areas.Admin.Models;

namespace Commerce.UnitTests.Controllers.Admin
{
    [TestFixture]
    public class AdminManagerControllerTests
    {
        public AggregateUser AdminUserFactory()
        {
            return new AggregateUser
            {
                ID = 999,
                IdentityProfile = new IdentityProfile
                {
                    AccountLevel = AccountLevel.NotApplicable,
                    AccountStatus = AccountStatus.Active,
                    FirstName = "John",
                    LastName = "Fathers",
                    UserRole = UserRole.Admin,
                },
                Membership = new PfMembershipUser
                {
                    CreationDate = DateTime.Now.AddDays(-7),
                    Email = "john@gmail.com",
                    IsApproved = true,
                    IsLockedOut = false,
                    IsOnline = false,
                    LastModified = DateTime.Now.AddHours(-3),
                }
            };
        }

        [Test]
        public void List_HttpPost()
        {
            // Arrange
            var aggregateUserRepository = MockRepository.GenerateMock<IReadOnlyAggregateUserRepository>();
            aggregateUserRepository 
                .Expect(x => x.Retreive(new List<UserRole>() { UserRole.Admin, UserRole.Root }))
                .Return(new List<AggregateUser> 
                        { 
                            AdminUserFactory(), 
                            AdminUserFactory(), 
                            AdminUserFactory()
                        });

            var adminmgrController = new ManagerController(aggregateUserRepository, null, null, null);

            // Act
            var result = adminmgrController.List();

            // Assert
            result.ShouldBeView();
            aggregateUserRepository.VerifyAllExpectations();
        }

        [Test]
        public void Detail_HttpGet()
        {
            // Arrange
            var aggregateUserRepository = MockRepository.GenerateMock<IReadOnlyAggregateUserRepository>();

            aggregateUserRepository
                .Expect(x => x.RetrieveById(123))
                .Return(AdminUserFactory());
            
            var adminmgrController = new ManagerController(aggregateUserRepository, null, null, null);


            // Act
            var result = adminmgrController.Details(123);

            // Assert
            result.ShouldBeView();
            aggregateUserRepository.VerifyAllExpectations();
        }

        [Test]
        public void Create_HttpPost()
        {
            // Arrange
            var model = new CreateAdminModel
            {
                Email = "test@test.com",
                FirstName = "Al",
                IsApproved = true,
                LastName = "Capone",
                Password = "123",
                PasswordVerify = "1234"
            };

            string status;
            
            var aggrService = MockRepository.GenerateMock<IAggregateUserService>();
            aggrService
                .Expect(x => x.Create(null, null, out status))
                .IgnoreArguments()
                .Return(new AggregateUser() {ID = 123});


            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            unitOfWork.Expect(x => x.SaveChanges());
            var controller = new ManagerController(null, aggrService, null, unitOfWork);

            // Act
            var result = controller.Create(model);

            // Assert
            aggrService.VerifyAllExpectations();
            unitOfWork.VerifyAllExpectations();
        }

        [Test]
        public void Edit_HttpPost_TestEditInvalidState()
        {
            // Arrange
            var controller = new ManagerController(null, null, null, null);
            controller.ModelState.AddModelError("", "It's all messed up!");

            // Act
            var result = controller.Edit(123, new EditUserModel());

            // Assert
            result.ShouldBeDefaultView();
        }

        // TODO: expand/deepen this test coverage
        [Test]
        public void Edit_HttpPost_ValidState()
        {
            // Arrange
            var expectedID = 888;
            var username = "123456";
            var expectedApproval = true;
            var expectedEmail = "tony@griepwffe.com";
            var expectedFirstName = "Tony";
            var expectedLastName = "Stark";
                 
            var user = new AggregateUser
            {
                ID = expectedID,
                Membership = new PfMembershipUser { UserName = username, Email = "tony@oldaddress.com"},
                IdentityProfile = new IdentityProfile(),
            };

            var repository = MockRepository.GenerateMock<IReadOnlyAggregateUserRepository>();
            repository
                .Expect(x => x.RetrieveById(expectedID))
                .Return(user);

            var aggregateUserService = MockRepository.GenerateMock<IAggregateUserService>();
            aggregateUserService.Expect(x => x.UpdateIdentity(888, null)).IgnoreArguments()
                                .Do((Action<int, IdentityProfileChange>) ((id, change) =>
                                    {
                                        Assert.That(id, Is.EqualTo(expectedID));
                                        Assert.That(change.FirstName, Is.EqualTo(expectedFirstName));
                                        Assert.That(change.LastName, Is.EqualTo(expectedLastName));
                                    }));

            var membershipService = MockRepository.GenerateMock<IPfMembershipService>();
            membershipService.Expect(x => x.SetUserApproval(username, expectedApproval));
            membershipService
                .Expect(x => x.ChangeEmailAddress(username, null, expectedEmail, true))
                .Return(PfCredentialsChangeStatus.Success);

            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            unitOfWork.Expect(x => x.SaveChanges());


            // Act
            var controller = new ManagerController(repository, aggregateUserService, membershipService, unitOfWork);
            var result = controller.Edit(expectedID, new EditUserModel()
                {
                    FirstName = expectedFirstName,
                    LastName = expectedLastName,
                    IsApproved = expectedApproval,
                    Email = expectedEmail,
                });

            // Assert
            result.ShouldBeRedirectionTo(new { action = "Details" });

            repository.VerifyAllExpectations();
            aggregateUserService.VerifyAllExpectations();
            membershipService.VerifyAllExpectations();
            unitOfWork.VerifyAllExpectations();
        }

        [Test]
        public void ChangePassword_HttpGet()
        {
            var repository = MockRepository.GenerateMock<IReadOnlyAggregateUserRepository>();
            repository
                .Expect(x => x.RetrieveById(888))
                .Return(new AggregateUser { Membership = new PfMembershipUser { Email = "hull@hello.com" } });
            var controller = new ManagerController(repository, null, null, null);

            var result = controller.ChangePassword(888);

            result.ShouldBeView();
            repository.VerifyAllExpectations();
        }

        [Test]
        public void Change_HttpPost_ValidState()
        {
            var service = MockRepository.GenerateMock<IPfMembershipService>();
            service.Expect(x => x.ChangePassword("888", "pass123", "pass456", false))
                   .Return(PfCredentialsChangeStatus.Success);
            var repository = MockRepository.GenerateMock<IReadOnlyAggregateUserRepository>();
            repository.Expect(
                x => x.RetrieveById(888))
                .Return(new AggregateUser() { Membership = new PfMembershipUser()
                {
                    UserName = "888"
                }});
            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            unitOfWork.Expect(x => x.SaveChanges());

            var controller = new ManagerController(repository, null, service, unitOfWork);
            var result = controller.ChangePassword(888, new ChangePasswordModel() { OldPassword = "pass123", NewPassword = "pass456" });

            result.ShouldBeRedirectionTo(new { Action = "Details" });
            service.VerifyAllExpectations();
        }

        [Test]
        public void Change_HttpPost_InvalidState()
        {
            var controller = new ManagerController(null, null, null, null);
            controller.ModelState.AddModelError("what", "don't type that in, son!");
            
            var result = controller.ChangePassword(888, new ChangePasswordModel());

            result.ShouldBeView();
        }

        [Test]
        public void Reset_HttpPost()
        {
            var repository = MockRepository.GenerateMock<IReadOnlyAggregateUserRepository>();
            repository
                .Expect(x => x.RetrieveById(888))
                .Return(new AggregateUser { Membership = new PfMembershipUser { UserName = "123456" } });

            var service = MockRepository.GenerateMock<IPfMembershipService>();
            PfCredentialsChangeStatus status;
            service
                .Expect(x => x.ResetPassword("123456", null, false, out status))
                .IgnoreArguments()
                .Return("seruifjk");

            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            unitOfWork.Expect(x => x.SaveChanges());
            var controller = new ManagerController(repository, null, service, unitOfWork);

            // Act
            var result = controller.Reset(888);

            // Assert
            result.ShouldBeView();
            repository.VerifyAllExpectations();
            service.VerifyAllExpectations();
            unitOfWork.VerifyAllExpectations();
        }

        [Test]
        public void Unlock_HttpGet()
        {
            var repository = MockRepository.GenerateMock<IReadOnlyAggregateUserRepository>();
            repository
                .Expect(x => x.RetrieveById(888))
                .Return(new AggregateUser { Membership = new PfMembershipUser { UserName = "123456" } });

            var service = MockRepository.GenerateMock<IPfMembershipService>();
            service.Expect(x => x.UnlockUser("123456"));

            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            unitOfWork.Expect(x => x.SaveChanges());

            // Act
            var controller = new ManagerController(repository, null, service, unitOfWork);
            var result = controller.Unlock(888);

            // Assert
            result.ShouldBeRedirectionTo(new { Action = "Details" });
            repository.VerifyAllExpectations();
            service.VerifyAllExpectations();
            unitOfWork.VerifyAllExpectations();
        }

        [Test]
        public void Delete_HttpGet()
        {
            var repository = MockRepository.GenerateMock<IReadOnlyAggregateUserRepository>();
            repository
                .Expect(x => x.RetrieveById(888))
                .Return(new AggregateUser 
                { 
                    Membership = new PfMembershipUser { UserName = "123456" },
                    IdentityProfile = new IdentityProfile(),
                });

            var controller = new ManagerController(repository, null, null, null);

            var result = controller.Delete(888);

            result.ShouldBeView();
            repository.VerifyAllExpectations();
        }

        [Test]
        public void DeleteConfirm_HttpPost()
        {
            // Arrange
            var service = MockRepository.GenerateMock<IAggregateUserService>();
            service.Expect(x => x.Delete(888));

            var unitOfWork = MockRepository.GenerateMock<IUnitOfWork>();
            unitOfWork.Expect(x => x.SaveChanges());

            var controller = new ManagerController(null, service, null, unitOfWork);

            // Act
            var result = controller.DeleteConfirm(888);

            // Assert
            result.ShouldBeRedirectionTo(new { Action = "List" });
            service.VerifyAllExpectations();
            unitOfWork.VerifyAllExpectations();
        }
    }
}