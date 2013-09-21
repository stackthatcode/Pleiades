using System;
using System.Net;
using System.Web.Mvc;
using Commerce.Web.Plumbing;

namespace Commerce.Web.Areas.Public.Controllers
{
    public class PageController : Controller
    {
        //
        // TODO: devise a better page navigation schema
        //
        public ActionResult Index()
        {
            return View();
        }

        // Actually, aren't these partial views...?  Hmmm?
        public ActionResult Contact()
        {
            throw new NotImplementedException();
        }

        public ActionResult About()
        {
            throw new NotImplementedException();
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

        // TODO: add System Downtime page stuff
    }
}
