using System;
using System.Web;
using System.Web.Mvc;
using Pleiades.Web.Security.Aspect;
using Pleiades.Web.Security.Model;
using Commerce.Web.Plumbing;

namespace Commerce.Web.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}