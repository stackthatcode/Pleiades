﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Web.Mvc;
using Commerce.Application.Email;
using Commerce.Application.Security;
using Commerce.Web.Models.Auth;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Model;

namespace Commerce.Web.Controllers
{
    public class UnsecuredController : Controller
    {
        public const bool PersistentCookie = true;
        public IAggregateUserService AggregateUserService { get; set; }
        public IFormsAuthenticationService FormsAuthenticationService { get; set; }
        
        public UnsecuredController(
                IAggregateUserService aggregateUserService, 
                IFormsAuthenticationService formsAuthenticationService)
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
                model.UserName, model.Password, PersistentCookie, new List<UserRole> { UserRole.Root, UserRole.Admin });
                        
            if (result == null)
            {
                ModelState.AddModelError("", "Failed authentication credentials");
                return View();
            }

            if (returnUrl != null)
            {
                var url = HttpContext.Server.UrlDecode(returnUrl);
                return Redirect(url);
            }

            return new RedirectToRouteResult(Navigation.Home());
        }

        [HttpGet]
        public ActionResult Logout()
        {
            this.FormsAuthenticationService.ClearAuthenticationCookie();
            return new RedirectToRouteResult(Navigation.Login());
        }

        public ActionResult ServerError()
        {
            this.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View();
        }

    }
}