using System.Web.Mvc;

namespace Pleiades.Web.MvcHelpers
{
    public static class StyleTagGenerator
    {        
        public static MvcHtmlString Stylesheet(this HtmlHelper html, string filePath)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var absoluteUrl = urlHelper.Content(filePath);

            var styleTag = new TagBuilder("link");
            styleTag.Attributes["rel"] = "Stylesheet";
            styleTag.Attributes["type"] = "text/css";
            styleTag.Attributes["media"] = "screen";
            styleTag.Attributes["href"] = absoluteUrl;
            
            return MvcHtmlString.Create(styleTag.ToString());
        }
    }
}