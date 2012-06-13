using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Pleiades.Commerce.WebUI.Areas.Admin.Models;
using Pleiades.Framework.Web.Security.Interface;
using Pleiades.Framework.Web.Security.Concrete;
using Pleiades.Framework.Web.Security.Model;


namespace Pleiades.Commerce.WebUI.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        public readonly bool PersistentCookie = true;
        public IDomainUserService UserService { get; set; }
        public IFormsAuthenticationService FormsAuthService { get; set; }
        public IMembershipService MembershipService { get; set; }


        // TODO: replace with Dependency Injection
        public AccountController()
        {
            FormsAuthService = new FormsAuthenticationService();
            UserService = new DomainUserService();
            MembershipService = new MembershipService();
        }

        [HttpGet]
        public ActionResult Logon()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logon(LogOnViewModel model, string returnUrl)
        {
            // 1.) Is the Model in a Valid State?
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed authentication credentials"); 
                return View();
            }

            // 2.) Can we validate the User with these credentials?
            var validateduser = this.MembershipService.ValidateUserByEmailAddr(model.UserName, model.Password);
            if (validateduser == null)
            {
                this.FormsAuthService.ClearAuthenticationCookie();
                ModelState.AddModelError("", "Failed authentication credentials");
                return View();
            }
            
            // Success - set the Form Authentication Cookie
            this.FormsAuthService.SetAuthCookieForUser(validateduser, this.PersistentCookie);
            if (returnUrl != null)
                return Redirect(returnUrl);
            else
                return new RedirectToRouteResult
                    (new RouteValueDictionary(new { area = "Admin", controller = "Home", action = "Index" }));
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthService.ClearAuthenticationCookie();
            return new RedirectToRouteResult(
                    new RouteValueDictionary(new { area = "", controller = "Products", action = "List" }));
        }
    }
}
