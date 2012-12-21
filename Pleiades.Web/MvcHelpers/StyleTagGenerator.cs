using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Pleiades.Web.MvcHelpers
{
    public static class StyleTagGenerator
    {
        public static string BasePath = "~/Content/";

        public static MvcHtmlString Stylesheet(this HtmlHelper html, string filePath)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var absoluteUrl = urlHelper.Content(BasePath + filePath);

            var styleTag = new TagBuilder("link");
            styleTag.Attributes["rel"] = "Stylesheet";
            styleTag.Attributes["type"] = "text/css";
            styleTag.Attributes["media"] = "screen";
            styleTag.Attributes["href"] = absoluteUrl;
            
            return MvcHtmlString.Create(styleTag.ToString());
        }
    }
}