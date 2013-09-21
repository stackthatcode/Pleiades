using System.Web;
using System.Web.Mvc;

namespace Pleiades.Web.MvcHelpers
{
    public static class UrlExtensions
    {
        public static string BaseUrl(this UrlHelper helper)
        {
            var request = HttpContext.Current.Request;
            return string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, helper.Content("~"));
        }
    }
}
