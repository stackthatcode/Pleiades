using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Pleiades.Execution;
using Pleiades.Injection;
using Pleiades.Web.Security.Execution.Context;
using Pleiades.Web.Security.Execution.Step;
using Pleiades.Web.Security.Model;
using Commerce.WebUI.Areas.Admin.Models;
using Commerce.WebUI.Plumbing.Navigation;

namespace Commerce.WebUI.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        public readonly bool PersistentCookie = true;
        public IGenericContainer Container { get; set; }

        public AuthController(IGenericContainer container)
        {
            this.Container = container;
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

            var context = new AuthenticateUserByRoleContext
            {
                AttemptedUserName = model.UserName,
                AttemptedPassword = model.Password,
                ExpectedRoles = new List<UserRole> { UserRole.Supreme, UserRole.Admin },
                PersistenceCookie = this.PersistentCookie,
            };

            var step = this.Container.Resolve<AuthenticateUserByRoleStep>();
            
            // NOTE: checking the returned result simplifies testing
            var result = step.Execute(context); 
            
            if (result.IsExecutionStateValid == false)
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
            var step = this.Container.Resolve<LogoutStep>();
            step.Execute(new BareContext());
            return new RedirectToRouteResult(OutboundNavigation.PublicHome());
        }
    }
}