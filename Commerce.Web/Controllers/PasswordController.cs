using System.Web.Mvc;
using Commerce.Application.Security;
using Commerce.Web.Models.Auth;
using Pleiades.Web.Security.Interface;

namespace Commerce.Web.Controllers
{
    public class PasswordController : Controller
    {
        private IPasswordResetLinkRepository PasswordResetLinkRepository { get; set; }
        private IMembershipReadOnlyRepository MembershipReadOnlyRepository { get; set; }

        public PasswordController(
                IPasswordResetLinkRepository passwordResetLinkRepository, 
                IMembershipReadOnlyRepository membershipReadOnlyRepository)
        {
            PasswordResetLinkRepository = passwordResetLinkRepository;
            MembershipReadOnlyRepository = membershipReadOnlyRepository;
        }


        [HttpGet]
        public ActionResult PasswordRequest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PasswordRequest(PasswordRequestModel model)
        {
            // Notice this is specific to System Admins...
            var account = MembershipReadOnlyRepository.GetUserByEmail(model.Email);
            if (account == null)
            {
                ModelState.AddModelError("", "Invalid Email Address");
                return View(model);
            }

            var url = "";

            // Build Email for Reset
            // Send Email for Reset

            return RedirectToAction("PasswordRequestSuccess");
        }

        [HttpGet]
        public ActionResult PasswordRequestSuccess()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PasswordChange(string linkId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult PasswordChange()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PasswordChangeSuccess()
        {
            return View();
        }



    }
}
