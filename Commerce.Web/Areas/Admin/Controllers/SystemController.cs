using System.Net;
using System.Web.Mvc;
using Commerce.Web.Plumbing;

namespace Commerce.Web.Areas.Admin.Controllers
{
    public class SystemController : Controller
    {
        //
        // GET: /Admin/System/

        public ActionResult ServerError()
        {
            this.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View();
        }
    }
}
