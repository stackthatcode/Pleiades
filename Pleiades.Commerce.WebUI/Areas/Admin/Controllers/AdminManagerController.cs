using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Pleiades.Commerce.WebUI.Areas.Admin.Models;
using Pleiades.Commerce.WebUI.Plumbing.Security;
using Pleiades.Commerce.WebUI.Plumbing.UI.StandardHtmlHelpers;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Concrete;
using Pleiades.Framework.Web.Security.Model;
using Pleiades.Web.Security.Utility;
using Pleiades.Framework.Helpers;


namespace Pleiades.Commerce.WebUI.Areas.Admin.Controllers
{
    [ConcreteAuthorize(AuthorizationZone = AuthorizationZone.Administrative)]
    public class AdminManagerController : Controller
    {
        public const int PageSize = 10;
        public const string DefaultQuestion = "Type Default";
        public const string DefaultAnswer = "Default";

        public IDomainUserService DomainUserService { get; set; }
        public IMembershipService MembershipService { get; set; }

        public AdminManagerController()
        {
            // TODO: wire the Autofac IoC Plumbing into this, heah!
            DomainUserService = new DomainUserService();
            MembershipService = new MembershipService();
        }

        [HttpGet]
        public ActionResult List(int page = 1)
        {
            var users = DomainUserService.RetreiveAll(page, PageSize, new List<UserRole>() { UserRole.Admin, UserRole.Root });
            return View(new ListUsersViewModel { Users = users });
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var user = DomainUserService.RetrieveUserByDomainUserId(id);
            var userModel = UserViewModel.Make(user);
            return View(userModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new CreateAdminModel { IsApproved = false });
        } 

        [HttpPost]
        public ActionResult Create(CreateAdminModel createAdminModel)
        {
            if (!ModelState.IsValid)
                return View(createAdminModel);

            // Some way to validate args sent Service en-masse.... like passing a context or something

            MembershipCreateStatus status;

            var newuser = 
                this.DomainUserService.Create(
                    new CreateNewUserRequest
                    {
                        Password = createAdminModel.Password,
                        Email = createAdminModel.Email,
                        PasswordQuestion = DefaultQuestion,
                        PasswordAnswer = DefaultAnswer,
                        AccountStatus = AccountStatus.Active,
                        UserRole = UserRole.Admin,
                        AccountLevel = AccountLevel.Standard,
                        FirstName = createAdminModel.FirstName,
                        LastName =  createAdminModel.LastName
                    },
                    out status);

            if (status != MembershipCreateStatus.Success)
            {
                ViewData[ViewDataKeys.ErrorMessage] = "Error creating user with following code: " + status.ToString();
                return View(createAdminModel);
            }

            return RedirectToAction("Details", new { id = newuser.DomainUserId });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = DomainUserService.RetrieveUserByDomainUserId(id);
            var userModel = UserViewModel.Make(user);
            return View(userModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, UserViewModel userViewModel)
        {
            // This is an interesting issue here, neh...?  Don't save this data on the client side, no matter what!
            var dbDomainUser = DomainUserService.RetrieveUserByDomainUserId(id);
            userViewModel.UserName = dbDomainUser.MembershipUser.UserName;

            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }

            // Update Identity User stuff
            dbDomainUser.FirstName = userViewModel.FirstName;
            dbDomainUser.LastName = userViewModel.LastName;
            DomainUserService.Update(dbDomainUser);

            // Update Membership stuff
            if (dbDomainUser.UserRole != UserRole.Root)
            {
                MembershipService.SetUserApproval(dbDomainUser, userViewModel.IsApproved);
            }

            // Leave Email Disabled for now
            if (userViewModel.Email != dbDomainUser.MembershipUser.Email)
            {
                MembershipService.ChangeEmailAddress(dbDomainUser, userViewModel.Email);
            }
            
            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public ActionResult Change(int id)
        {
            var user = DomainUserService.RetrieveUserByDomainUserId(id);
            return View(new ChangePasswordModel { Email = user.MembershipUser.Email });
        }

        [HttpPost]
        public ActionResult Change(int id, ChangePasswordModel model)
        {
            if (!this.ModelState.IsValid)
                return View(model);

            var user = DomainUserService.RetrieveUserByDomainUserId(id);
            MembershipService.ChangePassword(user, model.OldPassword, model.NewPassword);

            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public ActionResult Reset(int id)
        {
            var user = DomainUserService.RetrieveUserByDomainUserId(id);
            var newpassword = this.MembershipService.ResetPassword(user);

            return View(
                new ResetPasswordModel { 
                    Email = user.MembershipUser.Email, NewPassword = newpassword });
        }

        [HttpGet]
        public ActionResult Unlock(int id)
        {
            var user = DomainUserService.RetrieveUserByDomainUserId(id);
            MembershipService.UnlockUser(user);
            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var user = DomainUserService.RetrieveUserByDomainUserId(id);
            var userModel = UserViewModel.Make(user);
            return View(userModel);
        }

        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            var user = DomainUserService.RetrieveUserByDomainUserId(id);
            DomainUserService.Delete(user);

            return RedirectToAction("List");
        }
    }
}
