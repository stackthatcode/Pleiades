using System;
using System.Configuration;
using System.Web.Mvc;
using Commerce.Application.Email;
using Commerce.Application.Security;
using Commerce.Web.Models.Auth;
using Pleiades.App.Data;
using Pleiades.App.Logging;
using Pleiades.Web.Security.Interface;

namespace Commerce.Web.Controllers
{
    public class PasswordController : Controller
    {
        private readonly IPfMembershipService _pfMembershipService;
        private readonly IPasswordResetLinkRepository _passwordResetLinkRepository;
        private readonly IMembershipReadOnlyRepository _membershipReadOnlyRepository;
        private readonly IAdminEmailBuilder _adminEmailBuilder;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        private const int LinkExpirationMinutes = 15;

        public PasswordController(
                IPfMembershipService pfMembershipService,
                IPasswordResetLinkRepository passwordResetLinkRepository, 
                IMembershipReadOnlyRepository membershipReadOnlyRepository,
                IAdminEmailBuilder adminEmailBuilder,
                IEmailService emailService,
                IUnitOfWork unitOfWork)
        {
            _pfMembershipService = pfMembershipService;
            _passwordResetLinkRepository = passwordResetLinkRepository;
            _membershipReadOnlyRepository = membershipReadOnlyRepository;
            _adminEmailBuilder = adminEmailBuilder;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult PasswordRequest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PasswordRequest(PasswordRequestModel model)
        {
            var membershipUser = _membershipReadOnlyRepository.GetUserByEmail(model.Email);
            if (membershipUser == null || membershipUser.IsLockedOut || !membershipUser.IsApproved)
            {
                ModelState.AddModelError("", "Invalid Email Address");
                return View(model);
            }
            
            var resetLink = new PasswordResetLink
            {
                ExternalGuid = Guid.NewGuid(),
                MembershipUserName = membershipUser.UserName,
                DateCreated = DateTime.Now,
                ExpirationDate = DateTime.Now.AddMinutes(LinkExpirationMinutes),
            };

            var url = ConfigurationManager.AppSettings["AdminUrl"] +
                        "/Password/Change?ExternalGuid=" + resetLink.ExternalGuid;
            LoggerSingleton.Get().Info(url);

            _passwordResetLinkRepository.Add(resetLink);
            _unitOfWork.SaveChanges();

            var message = _adminEmailBuilder.PasswordReset(model.Email, url);
            _emailService.Send(message);

            return RedirectToAction("PasswordRequestSuccess");
        }

        [HttpGet]
        public ActionResult PasswordRequestSuccess()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Change(string externalGuid)
        {
            var guid = Guid.Parse(externalGuid);
            var link = _passwordResetLinkRepository.Retrieve(guid);
            if (link == null || link.Expired)
            {
                return RedirectToAction("BadLink");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Change(PasswordChangeModel model)
        {
            if (model.Password.Trim().Length < 6)
            {
                ModelState.AddModelError("", "Please enter a password with at least 6 characters");
                return View(model);
            }
            if (model.Password != model.PasswordConfirm)
            {
                ModelState.AddModelError("", "Please make your passwords match");
                return View(model);
            }

            var link = _passwordResetLinkRepository.Retrieve(Guid.Parse(model.ExternalGuid));
            if (link == null || link.Expired)
            {
                return RedirectToAction("BadLink");
            }

            var member = _membershipReadOnlyRepository.GetUserByUserName(link.MembershipUserName);
            _pfMembershipService.ChangePassword(member.UserName, null, model.Password, true);
            _unitOfWork.SaveChanges();
            return RedirectToAction("ChangeSuccess");
        }

        [HttpGet]
        public ActionResult ChangeSuccess()
        {
            return View();
        }


        [HttpGet]
        public ActionResult BadLink()
        {
            return View();
        }
    }
}
