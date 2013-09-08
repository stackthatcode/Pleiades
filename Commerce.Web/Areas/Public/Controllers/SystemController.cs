using System;
using System.Net;
using System.Web.Mvc;
using Commerce.Web.Areas.Public.Models;
using Commerce.Web.Plumbing;

namespace Commerce.Web.Areas.Public.Controllers
{
    public class SystemController : Controller
    {
        public ActionResult Exception()
        {
            throw new Exception();
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
            return View("~/Areas/Public/Views/System/NotFound.cshtml");
        }

        public ActionResult Contact()
        {
            return View();
        }

        // TODO: add System Downtime page stuff

    }
}
