using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Commerce.Web.Areas.Admin.Models;
using Pleiades.Application.Data;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.Web.Areas.Admin.Controllers
{
    public class ManagerController : Controller
    {
        public const string DefaultQuestion = "Type Default";
        public const string DefaultAnswer = "Default";

        public IReadOnlyAggregateUserRepository AggregateUserRepository { get; set; }
        public IAggregateUserService AggregateUserService { get; set; }
        public IPfMembershipService MembershipService { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }

        public ManagerController(
                IReadOnlyAggregateUserRepository aggregateUserRepository, 
                IAggregateUserService aggregateUserService,
                IPfMembershipService membershipService,
                IUnitOfWork unitOfWork)
        {
            AggregateUserRepository = aggregateUserRepository;
            AggregateUserService = aggregateUserService;
            MembershipService = membershipService;
            UnitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult List(int page = 1)
        {
            var users = AggregateUserRepository.Retreive(new List<UserRole>() { UserRole.Admin, UserRole.Root });
            var listUsersViewModel = users.Select(user => new UserViewModel(user)).ToList();
            return View(listUsersViewModel); 
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
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

            string status;
            var newuser =
                this.AggregateUserService.Create(
                    new PfCreateNewMembershipUserRequest
                    {
                        Email = createAdminModel.Email,
                        IsApproved = true,
                        Password = createAdminModel.Password,
                        PasswordAnswer = DefaultAnswer,
                        PasswordQuestion = DefaultQuestion,
                    },
                    new IdentityProfileChange
                    {                        
                        AccountStatus = AccountStatus.Active,
                        UserRole = UserRole.Admin,
                        AccountLevel = AccountLevel.Standard,
                        FirstName = createAdminModel.FirstName,
                        LastName =  createAdminModel.LastName
                    },
                    out status);
            this.UnitOfWork.SaveChanges();

            if (newuser == null)
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
                new IdentityProfileChange 
                { 
                    FirstName = userViewModel.FirstName, 
                    LastName = userViewModel.LastName, 
                });
            this.UnitOfWork.SaveChanges();

            // Respond
            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public ActionResult ChangePassword(int id)
        {
            var user = this.AggregateUserRepository.RetrieveById(id);
            return View(new ChangePasswordModel { Email = user.Membership.Email });
        }

        [HttpPost]
        public ActionResult ChangePassword(int id, ChangePasswordModel model)
        {
            // Validate
            if (!this.ModelState.IsValid) 
                return View(model);

            // Execute
            var user = this.AggregateUserRepository.RetrieveById(id);
            this.MembershipService.ChangePassword(user.Membership.UserName, model.OldPassword, model.NewPassword, false);
            this.UnitOfWork.SaveChanges();

            // Respond
            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public ActionResult Reset(int id)
        {
            var user = this.AggregateUserRepository.RetrieveById(id);
            PfCredentialsChangeStatus status;
            var newpassword = this.MembershipService.ResetPassword(user.Membership.UserName, null, true, out status);
            this.UnitOfWork.SaveChanges();
            return View(new ResetPasswordModel { Email = user.Membership.Email, NewPassword = newpassword });
        }

        [HttpGet]
        public ActionResult Unlock(int id)
        {
            var user = this.AggregateUserRepository.RetrieveById(id);
            MembershipService.UnlockUser(user.Membership.UserName);
            this.UnitOfWork.SaveChanges();
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
            this.AggregateUserService.Delete(id);
            this.UnitOfWork.SaveChanges();
            return RedirectToAction("List");
        }
    }
}