using System;
using System.Net;
using System.Web.Mvc;
using ArtOfGroundFighting.Web.Plumbing;

namespace ArtOfGroundFighting.Web.Controllers
{
    public class PageController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ServerError()
        {
            var model = new ErrorModel();
            if (this.HttpContext.Request.UrlReferrer != null &&
                this.HttpContext.Request.UrlReferrer.ToString().Contains("/Admin"))
            {
                model.NavigatedFromAdminArea = true;
            }

            if (this.HttpContext.Request.QueryString["aspxerrorpath"] != null)
            {
                model.AspxErrorPath = this.HttpContext.Request.QueryString["aspxerrorpath"];
                if (this.HttpContext.Request.QueryString["aspxerrorpath"].Contains("/Admin"))
                {
                    model.NavigatedFromAdminArea = true;
                }
            }

            this.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View(model);
        }

        public ActionResult NotFound()
        {
            this.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View();
        }

        public ActionResult TestHttp500()
        {
            throw new Exception();
        }
    }
}
