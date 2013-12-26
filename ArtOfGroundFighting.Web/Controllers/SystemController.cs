using System.Web.Mvc;

namespace ArtOfGroundFighting.Web.Controllers
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
