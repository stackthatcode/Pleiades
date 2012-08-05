using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Pleiades.Framework.Execution;
using Pleiades.Framework.Injection;
using Pleiades.Framework.Identity.Model;
using Pleiades.Commerce.Web.Security.Execution.Steps;
using Pleiades.Commerce.Web.Security.Model;
using Pleiades.Commerce.WebUI.Areas.Admin.Models;

namespace Pleiades.Commerce.WebUI.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        public readonly bool PersistentCookie = true;
        public IGenericContainer Container { get; set; }
        public object StepContext { get; set; }

        public LoginController(IGenericContainer container)
        {
            this.Container = container;
        }

        [HttpGet]
        public ActionResult Logon()
        {
            return View();
        }

        // Here's the rub if we go with this design!
        //
        // 1.) We need to test Model to Context translation => http://stackoverflow.com/questions/6872447/using-rhino-mocks-how-to-check-the-value-of-a-struct-field-in-the-parameter-pass
        // 2.) We need to test Invalid Model returns empty View
        // 3.) We need to test IsExecutionStateValid == false
        // 4.) We need to test IsExecutionStateValid == true - ReturnUrl != ""
        // 5.) We need to test IsExecutionStateValid == true - ReturnUrl == ""

        // PROBLEMS - building solid, testable connection between Model and Context

        [HttpPost]
        public ActionResult Logon(LogOnViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed authentication credentials"); 
                return View();
            }

            this.StepContext = new AuthenticateUserByRoleContext
            {
                AttemptedUserName = model.UserName,
                AttemptedPassword = model.Password,
                ExpectedRoles = new List<UserRole> { UserRole.Supreme, UserRole.Admin },
                PersistenceCookie = this.PersistentCookie,
            };

            var step = this.Container.Resolve<AuthenticateUserByRoleStep>();
            var result = step.Execute((AuthenticateUserByRoleContext)StepContext);

            if (result.IsExecutionStateValid == false)
            {
                ModelState.AddModelError("", "Failed authentication credentials");
                return View();
            }

            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return new RedirectToRouteResult
                    (new RouteValueDictionary(new { area = "Admin", controller = "Home", action = "Index" }));
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            var step = this.Container.Resolve<LogoutStep>();
            step.Execute(new BareContext());
            return new RedirectToRouteResult(
                    new RouteValueDictionary(new { area = "Public", controller = "Products", action = "List" }));
        }
    }
}
