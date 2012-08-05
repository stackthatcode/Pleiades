using System;
using System.Web;
using System.Web.Mvc;
using Pleiades.Framework.Identity.Model;
using Pleiades.Commerce.Web.Security.Aspect;
using Pleiades.Commerce.Domain.Model.Users;

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
