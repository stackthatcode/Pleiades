using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;
using Commerce.WebUI.Areas.Admin.Models;
using Commerce.WebUI.Plumbing.Navigation;

namespace Commerce.WebUI.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        public const bool PersistentCookie = true;
        public IAggregateUserService AggregateUserService { get; set; }
        public IFormsAuthenticationService FormsAuthenticationService { get; set; }

        public AuthController(
                IAggregateUserService aggregateUserService, IFormsAuthenticationService formsAuthenticationService)
        {
            this.AggregateUserService = aggregateUserService;
            this.FormsAuthenticationService = formsAuthenticationService;
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("What?");
            }
            return View(new LogOnViewModel());
        }

        [HttpPost]
        public ActionResult Login(LogOnViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed authentication credentials"); 
                return View();
            }

            var result = this.AggregateUserService.Authenticate(
                model.UserName, model.Password, PersistentCookie, new List<UserRole> { UserRole.Supreme, UserRole.Admin });
                        
            if (result == false)
            {
                ModelState.AddModelError("", "Failed authentication credentials");
                return View();
            }

            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return new RedirectToRouteResult(OutboundNavigation.AdminHome());
        }

        [HttpGet]
        public ActionResult Logout()
        {
            this.FormsAuthenticationService.ClearAuthenticationCookie();
            return new RedirectToRouteResult(OutboundNavigation.PublicHome());
        }
    }
}