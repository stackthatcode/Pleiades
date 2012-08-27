using System;
using System.Web;
using System.Web.Mvc;
using Pleiades.Web.Security.Aspect;
using Pleiades.Web.Security.Model;
using Commerce.WebUI.Plumbing.Navigation;

namespace Commerce.WebUI.Areas.Admin.Controllers
{
    //[CommerceAuthorize(AuthorizationZone = AuthorizationZone.Administrative)]
    public class HomeController : Controller
    {
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
