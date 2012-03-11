﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pleiades.Commerce.WebUI.Plumbing.Security;
using Pleiades.Web.Security.Model;

namespace Pleiades.Commerce.WebUI.Areas.Admin.Controllers
{
    [ConcreteAuthorize(AuthorizationZone = AuthorizationZone.Administrative)]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult HeaderSummary(DomainUser user)
        {
            return View(user);
        }
    }
}
