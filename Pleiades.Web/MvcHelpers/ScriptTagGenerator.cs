﻿using System;
using System.Web.Mvc;

namespace Pleiades.Web.MvcHelpers
{
    public static class ScriptTagGenerator
    {
        public static string BasePath = "~/Content/";

        public static MvcHtmlString Javascript<T>(this HtmlHelper<T> html, string relativeFilePath)
        {
            // First create the 
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            var absoluteFileUrl = urlHelper.Content(BasePath + relativeFilePath);

            // Next, build the script tag
            var scriptTag = new TagBuilder("script");
            scriptTag.Attributes["type"] = "text/javascript";
            scriptTag.Attributes["src"] = absoluteFileUrl;

            return MvcHtmlString.Create(scriptTag.ToString());
        }
    }
}