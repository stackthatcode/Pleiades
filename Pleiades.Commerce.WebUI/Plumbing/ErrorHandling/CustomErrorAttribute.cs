using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;

namespace Pleiades.Commerce.WebUI.Plumbing.ErrorHandling
{
    public class CustomErrorAttribute :  HandleErrorAttribute
    {
        // Summary:
        //     Called when an exception occurs.
        //
        // Parameters:
        //   filterContext:
        //     The action-filter context.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The filterContext parameter is null.
        public virtual void OnException(ExceptionContext filterContext)
        {
            Debug.WriteLine(filterContext.Exception.Message);
        }
    }
}