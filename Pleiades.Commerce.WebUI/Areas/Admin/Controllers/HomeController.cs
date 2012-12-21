using System;
using System.Web;
using System.Web.Mvc;
using Pleiades.Web.Security.Aspect;
using Pleiades.Web.Security.Model;
using Commerce.WebUI.Plumbing;

namespace Commerce.WebUI.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // TODO: just removed HttpGet attribute from this - what is *exact* theory behind that failure.  Hmmm???
        public ViewResult HeaderSummary(AggregateUser user)
        {
            return View(user);
        }
    }
}