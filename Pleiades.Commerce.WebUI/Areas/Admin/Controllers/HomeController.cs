using System;
using System.Web;
using System.Web.Mvc;
using Pleiades.Commerce.Web.Security.Aspect;
using Pleiades.Framework.Web.Security.Model;

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

        // TODO: just removed HttpGet attribute from this - what is *exact* theory behind that failure.  Hmmm???
        public ViewResult HeaderSummary(DomainUser user)
        {
            return View(user);
        }
    }
}
