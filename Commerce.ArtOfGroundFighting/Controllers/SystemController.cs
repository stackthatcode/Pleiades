﻿using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;

namespace Commerce.ArtOfGroundFighting.Controllers
{
    public class SystemController : Controller
    {
        public ActionResult ServerError()
        {
            return View("~/Views/Page/ServerError.cshtml");
        }

        public ActionResult NotFound()
        {
            return View("~/Views/Page/NotFound.cshtml");
        }
    }
}
