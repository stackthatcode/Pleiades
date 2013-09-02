using System.Web.Mvc;
using Pleiades.Application.Logging;

namespace Commerce.Web.Plumbing
{
    public class HandleErrorAttributeImpl : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            LoggerSingleton.Get().Error(filterContext.Exception);

            var controller = filterContext.Controller;
            //var action = filterContext.
        }
    }
}