using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Commerce.WebUI.Areas.Admin.Models;
using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Execution.Composites;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.WebUI.Areas.Admin.Controllers
{
    public class ManagerController : Controller
    {
        public const string DefaultQuestion = "Type Default";
        public const string DefaultAnswer = "Default";

        public IAggregateUserRepository AggregateUserRepository { get; set; }
        public IAggregateUserService AggregateUserService { get; set; }
        public IMembershipService MembershipService { get; set; }

        public ManagerController(
                IAggregateUserRepository aggregateUserRepository, 
                IAggregateUserService aggregateUserService,
                IMembershipService membershipService)
        {
            AggregateUserRepository = aggregateUserRepository;
            AggregateUserService = aggregateUserService;
            MembershipService = membershipService;
        }

        [HttpGet]
        public ActionResult List(int page = 1)
        {
            var users = AggregateUserRepository.Retreive(new List<UserRole>() { UserRole.Admin, UserRole.Supreme });
            var listUsersViewModel = users.Select(user => new UserViewModel(user)).ToList();
            return View(listUsersViewModel); 
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            // TODO: what if the User is null...? => GOTO THE ERROR PAGE

            var user = AggregateUserRepository.RetrieveById(id);
            var userModel = new UserViewModel(user);
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
            {
                return View(createAdminModel);
            }

            PleiadesMembershipCreateStatus status;
            var newuser =
                this.AggregateUserService.Create(
                    new CreateNewMembershipUserRequest
                    {
                        Email = createAdminModel.Email,
                        IsApproved = true,
                        Password = createAdminModel.Password,
                        PasswordAnswer = DefaultAnswer,
                        PasswordQuestion = DefaultQuestion,
                    },
                    new CreateOrModifyIdentityRequest
                    {                        
                        AccountStatus = AccountStatus.Active,
                        UserRole = UserRole.Admin,
                        AccountLevel = AccountLevel.Standard,
                        FirstName = createAdminModel.FirstName,
                        LastName =  createAdminModel.LastName
                    },
                    out status);

            if (status != PleiadesMembershipCreateStatus.Success)
            {
                ViewData["ErrorMessage"] = "Error creating user with following code: " + status.ToString();
                return View(createAdminModel);
            }

            return RedirectToAction("Details", new { id = newuser.ID });
        }
        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = this.AggregateUserRepository.RetrieveById(id);
            var userModel = new EditUserModel(user);
            return View(userModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, EditUserModel userViewModel)
        {
            // Validate
            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }

            // Execute
            this.AggregateUserService.UpdateIdentity(id, 
                    new CreateOrModifyIdentityRequest { FirstName = userViewModel.FirstName, LastName = userViewModel.LastName });

            // TODO: add Unit Test for this branch
            var user = this.AggregateUserRepository.RetrieveById(id);
            if (user.IdentityProfile.UserRole != UserRole.Supreme)
            {
                this.AggregateUserService.UpdateEmail(id, userViewModel.Email);
                this.AggregateUserService.UpdateApproval(id, userViewModel.IsApproved);
            }

            // Respond
            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public ActionResult Change(int id)
        {
            var user = this.AggregateUserRepository.RetrieveById(id);
            return View(new ChangePasswordModel { Email = user.Membership.Email });
        }

        [HttpPost]
        public ActionResult Change(int id, ChangePasswordModel model)
        {
            // Validate
            if (!this.ModelState.IsValid) 
                return View(model);

            // Execute
            this.AggregateUserService.ChangeUserPassword(id, model.OldPassword, model.NewPassword);

            // Respond
            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public ActionResult Reset(int id)
        {
            var user = this.AggregateUserRepository.RetrieveById(id);
            var newpassword = this.MembershipService.ResetPassword(user.Membership.UserName);

            return View(new ResetPasswordModel { Email = user.Membership.Email, NewPassword = newpassword });
        }

        [HttpGet]
        public ActionResult Unlock(int id)
        {
            var user = this.AggregateUserRepository.RetrieveById(id);
            MembershipService.UnlockUser(user.Membership.UserName);
            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var user = this.AggregateUserRepository.RetrieveById(id);
            var userModel = new UserViewModel(user);    
            return View(userModel);
        }

        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            this.AggregateUserRepository.Delete(id);
            return RedirectToAction("List");
        }
    }
}