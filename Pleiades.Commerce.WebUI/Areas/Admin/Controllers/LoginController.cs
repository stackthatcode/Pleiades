using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity.Model;
using Pleiades.Commerce.Web.Security.Execution.Steps;
using Pleiades.Commerce.Web.Security.Model;
using Pleiades.Commerce.WebUI.Areas.Admin.Models;
using Pleiades.Commerce.WebUI.Plumbing.Navigation;

namespace Pleiades.Commerce.WebUI.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        public readonly bool PersistentCookie = true;
        public IGenericContainer Container { get; set; }

        public LoginController(IGenericContainer container)
        {
            this.Container = container;
        }

        [HttpGet]
        public ActionResult Logon()
        {
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("What?");
            }
            return View(new LogOnViewModel());
        }

        [HttpPost]
        public ActionResult Logon(LogOnViewModel model, string returnUrl)
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

            return new RedirectToRouteResult(OutboundNavigation.AdminHome);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            var step = this.Container.Resolve<LogoutStep>();
            step.Execute(new BareContext());
            return new RedirectToRouteResult(OutboundNavigation.PublicHome);
        }
    }
}