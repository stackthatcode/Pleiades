using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.TestHelpers;
using Pleiades.TestHelpers.Web;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Commerce.Domain.Interface;
using Commerce.WebUI.Areas.Admin.Controllers;
using Commerce.WebUI.Areas.Admin.Models;

namespace Commerce.WebUI.TestsControllers
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
                Membership = new MembershipUser
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
        public void Verify_List_POST()
        {
            // Arrange
            var aggregateUserRepository = MockRepository.GenerateMock<IAggregateUserRepository>();

            aggregateUserRepository 
                .Expect(x => x.Retreive(new List<UserRole>() { UserRole.Admin, UserRole.Supreme }))
                .Return(new List<AggregateUser> 
                        { 
                            AdminUserFactory(), 
                            AdminUserFactory(), 
                            AdminUserFactory()
                        });

            var adminmgrController = new ManagerController(aggregateUserRepository, null, null);

            // Act
            var result = adminmgrController.List(1);

            // Assert
            result.ShouldBeView();
            aggregateUserRepository.VerifyAllExpectations();
        }

        [Test]
        public void Verify_Detail_GET()
        {
            // Arrange
            var aggregateUserRepository = MockRepository.GenerateMock<IAggregateUserRepository>();

            aggregateUserRepository
                .Expect(x => x.RetrieveById(123))
                .Return(AdminUserFactory());
            
            var adminmgrController = new ManagerController(aggregateUserRepository, null, null);


            // Act
            var result = adminmgrController.Details(123);

            // Assert
            result.ShouldBeView();
            aggregateUserRepository.VerifyAllExpectations();
        }

        // TODO: can we really justify writing a CRUD Test...????

        [Test]
        public void Verify_Create_POST()
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

            PleiadesMembershipCreateStatus status;
            
            var aggrService = MockRepository.GenerateMock<IAggregateUserService>();
            aggrService
                .Expect(x => x.Create(null, null, out status))
                .IgnoreArguments()
                .Return(new AggregateUser() { ID = 123 })
                .OutRef(PleiadesMembershipCreateStatus.Success);

            var controller = new ManagerController(null, aggrService, null);

            // Act
            var result = controller.Create(model);

            // Assert
            aggrService.VerifyAllExpectations();
        }

        [Test]
        public void Verify_Edit_POST_TestEditInvalidState()
        {
            // Arrange
            var controller = new ManagerController(null, null, null);
            controller.ModelState.AddModelError("", "It's all messed up!");

            // Act
            var result = controller.Edit(123, new UserViewModel());

            // Assert
            result.ShouldBeDefaultView();
        }

        [Test]
        public void Verify_Edit_POST_ValidState_NonSupreme()
        {
            // Arrange
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository.Expect(x => x.RetrieveById(123))
                .Return(new AggregateUser()
                    {
                        Membership = new MembershipUser { UserName = "789023", Email = "test@test.com" },
                        IdentityProfile = new IdentityProfile { UserRole = UserRole.Trusted },
                    });
            repository.Expect(x => x.UpdateIdentity(123, null)).IgnoreArguments();

            var membership = MockRepository.GenerateMock<IMembershipService>();
            membership.Expect(x => x.SetUserApproval("789023", true));
            membership.Expect(x => x.ChangeEmailAddress("789023", "test123@test.com"));

            var controller = new ManagerController(repository, null, membership);

            var input = new UserViewModel()
            {
                Email = "test123@test.com",
                IsApproved = true,                  
            };

            // Act
            var result = controller.Edit(123, new UserViewModel());

            // Assert
            result.ShouldBeRedirectionTo(new { action = "Details" });
            repository.VerifyAllExpectations();
        }

        //[Test]
        //public void TestChange()
        //{
        //    // Arrange
        //    var adminmgrController = new AdminManagerController();
        //    var domainUser = new DomainUser();
        //    adminmgrController.DomainUserService = MockRepository.GenerateMock<IDomainUserService>();
        //    adminmgrController.DomainUserService.Expect(x => x.RetrieveUserByDomainUserId(123)).Return(domainUser);
        //    adminmgrController.MembershipService = MockRepository.GenerateMock<IMembershipService>();
        //    adminmgrController.MembershipService.Expect(x => x.ChangePassword(domainUser, "123", "1234"));

        //    // Act
        //    var result = adminmgrController.Change(123,
        //        new ChangePasswordModel() { OldPassword = "123", NewPassword = "1234", PasswordVerify = "1234" });

        //    // Assert
        //    adminmgrController.DomainUserService.VerifyAllExpectations();
        //    result.ShouldBeRedirectionTo(new { action = "Details", id = 123 });
        //}

        //[Test]
        //public void TestUnlock()
        //{
        //    // Arrange
        //    var adminmgrController = new AdminManagerController();
        //    var domainUser = new DomainUser();
        //    adminmgrController.DomainUserService = MockRepository.GenerateMock<IDomainUserService>();
        //    adminmgrController.DomainUserService.Expect(x => x.RetrieveUserByDomainUserId(123)).Return(domainUser);
        //    adminmgrController.MembershipService = MockRepository.GenerateMock<IMembershipService>();
        //    adminmgrController.MembershipService.Expect(x => x.UnlockUser(domainUser));
            
        //    // Act
        //    var result = adminmgrController.Unlock(123);
                
        //    // Assert
        //    adminmgrController.DomainUserService.VerifyAllExpectations();
        //    result.ShouldBeRedirectionTo(new { action = "Details", id = 123 });
        //}

        //[Test]
        //public void TestDeleteConfirm()
        //{
        //    // Arrange
        //    var adminmgrController = new AdminManagerController();
        //    var domainUser = new DomainUser();
        //    adminmgrController.DomainUserService = MockRepository.GenerateMock<IDomainUserService>();
        //    adminmgrController.DomainUserService.Expect(x => x.RetrieveUserByDomainUserId(123)).Return(domainUser);
        //    adminmgrController.DomainUserService.Expect(x => x.Delete(domainUser));
            
        //    // Act
        //    var result = adminmgrController.DeleteConfirm(123);

        //    // Assert
        //    adminmgrController.DomainUserService.VerifyAllExpectations();
        //    result.ShouldBeRedirectionTo(new { action = "List" });
        //}
    
    }
}