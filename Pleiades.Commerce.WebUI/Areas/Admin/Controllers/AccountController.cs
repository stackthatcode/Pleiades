using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Pleiades.Commerce.WebUI.Areas.Admin.Models;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Concrete;
using Pleiades.Web.Security.Model;


namespace Pleiades.Commerce.WebUI.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        public readonly bool PersistentCookie = true;
        public IHttpContextUserService AuthService { get; set; }
        public IDomainUserService UserService { get; set; }

        // TODO: replace with Dependency Injection
        public AccountController()
        {
            AuthService = new HttpContextUserService();
            UserService = new DomainUserService();
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
            var validateduser = UserService.ValidateUserByEmailAddr(model.UserName, model.Password);
            if (validateduser == null)
            {
                AuthService.ClearAuthenticationCookie();
                ModelState.AddModelError("", "Failed authentication credentials");
                return View();
            }
            
            // Success - set the Form Authentication Cookie
            AuthService.SetAuthCookieForUser(validateduser, this.PersistentCookie);
            if (returnUrl != null)
                return Redirect(returnUrl);
            else
                return new RedirectToRouteResult
                    (new RouteValueDictionary(new { area = "Admin", controller = "Home", action = "Index" }));
        }

        [HttpGet]
        public ActionResult Logout()
        {
            AuthService.ClearAuthenticationCookie();
            return new RedirectToRouteResult(
                    new RouteValueDictionary(new { area = "", controller = "Products", action = "List" }));
        }
    }
}
