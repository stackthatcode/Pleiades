using System;
using System.Web;
using System.Web.Mvc;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.Web.Security.Aspect;
using Pleiades.Framework.Web.Security.Model;
using Pleiades.Commerce.WebUI.Plumbing.Navigation;

namespace Pleiades.Commerce.WebUI.Areas.Admin.Controllers
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
