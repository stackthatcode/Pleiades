using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Pleiades.App.Logging;
using Pleiades.App.Utility;
using Pleiades.Web.Activity;

namespace Commerce.Web.Plumbing
{
    public class HandleErrorAttributeImpl : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!ConfigurationManager.AppSettings["ErrorHandlingEnabled"].ToBoolTryParse())
            {
                return;
            }

            if (filterContext.ExceptionHandled) return;
            if (new HttpException(null, filterContext.Exception).GetHttpCode() != 500) return;
            if (!ExceptionType.IsInstanceOfType(filterContext.Exception)) return;

            // Log the Exception
            LoggerSingleton.Get().Error(
                    "URL:" + filterContext.HttpContext.Request.Url + " - " +
                    "IsAjaxRequest: " + filterContext.HttpContext.Request.IsAjaxRequest());
            LoggerSingleton.Get().Error(filterContext.Exception);

            // Notify System Admins
            ErrorNotification.Send(filterContext.Exception);

            // And now respond
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                        {
                            activityId = ActivityId.Current,
                            error = true,
                            message = "System Fault - check Logs for Activity Id"
                        }
                };
            }
            else
            {
                var model = new ErrorModel();                
                model.AspxErrorPath = filterContext.HttpContext.Request.Path;

                model.NavigatedFromAdminArea = true;

                filterContext.Result = new ViewResult
                {
                    ViewName = Navigation.ServerErrorView(),
                    ViewData = new ViewDataDictionary<ErrorModel>(model),
                };
            }

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}
