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

        //// TODO: can we really justify writing a CRUD Test...????
        //[Test]
        //public void TestCreate()
        //{
        //    // Arrange
        //    var model = new CreateAdminModel
        //    { 
        //        Email = "test@test.com", FirstName = "Al", IsApproved = true, LastName = "Capone", 
        //        Password = "123", PasswordVerify = "1234" 
        //    };

        //    var adminmgrController = new AdminManagerController();
        //    adminmgrController.DomainUserService = MockRepository.GenerateMock<IDomainUserService>();
        //    MembershipCreateStatus status;

        //    adminmgrController.DomainUserService
        //        .Expect(x => x.Create(
        //                new CreateNewUserRequest()
        //                {
        //                    Password = "123", 
        //                    Email = "test@test.com",
        //                    PasswordQuestion = AdminManagerController.DefaultQuestion,
        //                    PasswordAnswer = AdminManagerController.DefaultAnswer, 
        //                    IsApproved = true, 
        //                    AccountStatus = AccountStatus.Active, 
        //                    UserRole = UserRole.Admin,
        //                    AccountLevel = AccountLevel.Standard, 
        //                    FirstName = "Al", 
        //                    LastName = "Capone"
        //                }, out status))
        //        .IgnoreArguments()
        //        .Return(new DomainUser())
        //        .OutRef(MembershipCreateStatus.Success);

        //    // Act
        //    adminmgrController.Create(model);

        //    // Assert
        //    adminmgrController.DomainUserService.VerifyAllExpectations();
        //}

        //[Test]
        //public void TestEditInvalidState()
        //{
        //    // Arrange
        //    var adminmgrController = new AdminManagerController();
        //    adminmgrController.DomainUserService = MockRepository.GenerateMock<IDomainUserService>();
        //    adminmgrController.DomainUserService
        //        .Expect(x => x.RetrieveUserByDomainUserId(123))
        //        .Return(new DomainUser() { MembershipUser = new Web.Security.Model.MembershipUser() });
        //    adminmgrController.ModelState.AddModelError("", "It's all messed up!");

        //    // Act
        //    adminmgrController.Edit(123, new UserViewModel());

        //    // Assert
        //    adminmgrController.DomainUserService.VerifyAllExpectations();
        //}

        //// TODO: update this after adding the LeadRole
        //[Test]
        //[Ignore]
        //public void TestEditDontDisapproveLeadAdmin()
        //{
        //    // Arrange
        //    var adminmgrController = new AdminManagerController();

        //    var user = MockRepository.GenerateStub<DomainUser>();
        //    var membershipUser = MockRepository.GenerateStub<Web.Security.Model.MembershipUser>();
        //    user.MembershipUser = membershipUser;
        //    membershipUser.Stub(x => x.UserName).Return("admin");

        //    adminmgrController.DomainUserService = MockRepository.GenerateMock<IDomainUserService>();
        //    adminmgrController.DomainUserService
        //        .Expect(x => x.RetrieveUserByDomainUserId(123))
        //        .Return(user);
        //    adminmgrController.ModelState.AddModelError("", "It's all messed up!");

        //    // Act
        //    var result = adminmgrController.Edit(123, new UserViewModel() { IsApproved = false, });

        //    // Assert
        //    adminmgrController.DomainUserService.VerifyAllExpectations();
        //    result.ShouldBeView();
        //}

        //[Test]
        //public void TestEditValidState()
        //{
        //    // Arrange
        //    var adminmgrController = new AdminManagerController();

        //    var user = MockRepository.GenerateStub<DomainUser>();
        //    var membershipUser = MockRepository.GenerateStub<Web.Security.Model.MembershipUser>();
        //    user.MembershipUser = membershipUser;
        //    user.MembershipUser.IsApproved = true;

        //    adminmgrController.DomainUserService = MockRepository.GenerateMock<IDomainUserService>();
        //    adminmgrController.DomainUserService
        //        .Expect(x => x.RetrieveUserByDomainUserId(123))
        //        .Return(user);
        //    adminmgrController.DomainUserService.Expect(x => x.Update(user));

        //    adminmgrController.MembershipService = MockRepository.GenerateMock<IMembershipService>();
        //    adminmgrController.MembershipService.Expect(x => x.SetUserApproval(user, true));

        //    // Act
        //    var result = adminmgrController.Edit(123, new UserViewModel() { IsApproved = true, });

        //    // Assert
        //    adminmgrController.DomainUserService.VerifyAllExpectations();
        //    result.ShouldBeRedirectionTo(new { action = "Details", id = 123 } );
        //}

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