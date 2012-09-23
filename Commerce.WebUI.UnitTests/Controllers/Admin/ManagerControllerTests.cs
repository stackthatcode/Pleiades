﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.TestHelpers;
using Pleiades.TestHelpers.Web;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Execution.Composites;
using Pleiades.Web.Security.Execution.Context;
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
        public void Verify_Edit_POST_ValidState()
        {
            // Arrange
            var service = MockRepository.GenerateMock<IAggregateUserService>();
            service.Expect(x => x.UpdateIdentity(888, null)).IgnoreArguments();
            var controller = new ManagerController(null, service, null);
            
            // Act
            var result = controller.Edit(123, new UserViewModel());

            // Assert
            result.ShouldBeRedirectionTo(new { action = "Details" });
            service.VerifyAllExpectations();
        }

        [Test]
        public void Verify_Change_GET()
        {
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository
                .Expect(x => x.RetrieveById(888))
                .Return(new AggregateUser { Membership = new MembershipUser { Email = "hull@hello.com" } });
            var controller = new ManagerController(repository, null, null);

            var result = controller.Change(888);

            result.ShouldBeView();
            repository.VerifyAllExpectations();
        }

        [Test]
        public void Verify_Change_POST_ValidState()
        {
            var service = MockRepository.GenerateMock<IAggregateUserService>();
            service.Expect(x => x.ChangeUserPassword(888, "pass123", "pass456"));
            var controller = new ManagerController(null, service, null);

            var result = controller.Change(888, new ChangePasswordModel() { OldPassword = "pass123", NewPassword = "pass456" });

            result.ShouldBeRedirectionTo(new { Action = "Details" });
            service.VerifyAllExpectations();
        }

        [Test]
        public void Verify_Change_POST_InvalidState()
        {
            var controller = new ManagerController(null, null, null);
            controller.ModelState.AddModelError("what", "don't type that in, son!");
            
            var result = controller.Change(888, new ChangePasswordModel());

            result.ShouldBeView();
        }

        [Test]
        public void Verify_Reset_GET()
        {
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository
                .Expect(x => x.RetrieveById(888))
                .Return(new AggregateUser { Membership = new MembershipUser { UserName = "123456" } });

            var service = MockRepository.GenerateMock<IMembershipService>();
            service
                .Expect(x => x.ResetPassword("123456"))
                .Return("seruifjk");

            var controller = new ManagerController(repository, null, service);

            var result = controller.Reset(888);

            result.ShouldBeView();
            repository.VerifyAllExpectations();
            service.VerifyAllExpectations();
        }

        [Test]
        public void Verify_Unlock_GET()
        {
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository
                .Expect(x => x.RetrieveById(888))
                .Return(new AggregateUser { Membership = new MembershipUser { UserName = "123456" } });

            var service = MockRepository.GenerateMock<IMembershipService>();
            service.Expect(x => x.UnlockUser("123456"));

            var controller = new ManagerController(repository, null, service);

            var result = controller.Unlock(888);

            result.ShouldBeRedirectionTo(new { Action = "Details" });
            repository.VerifyAllExpectations();
            service.VerifyAllExpectations();
        }

        [Test]
        public void Verify_Delete_GET()
        {
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository
                .Expect(x => x.RetrieveById(888))
                .Return(new AggregateUser 
                { 
                    Membership = new MembershipUser { UserName = "123456" },
                    IdentityProfile = new IdentityProfile(),
                });

            var controller = new ManagerController(repository, null, null);

            var result = controller.Delete(888);

            result.ShouldBeView();
            repository.VerifyAllExpectations();
        }

        [Test]
        public void Verify_DeleteConfirm_POST()
        {
            var user = new AggregateUser
            {
                Membership = new MembershipUser { UserName = "123456" },
                IdentityProfile = new IdentityProfile(),
            };
            var repository = MockRepository.GenerateMock<IAggregateUserRepository>();
            repository
                .Expect(x => x.RetrieveById(888))
                .Return(user);
            repository.Expect(x => x.Delete(user));

            var controller = new ManagerController(repository, null, null);

            // Act
            var result = controller.DeleteConfirm(888);

            // Assert
            result.ShouldBeRedirectionTo(new { Action = "List" });
            repository.VerifyAllExpectations();
        }
    }
}