using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Pleiades.Web.MvcHelpers;

namespace Commerce.WebUI.QuickForm.CSharp
{
    public static class QfDependencyExtension
    {
        static string CssVirtualPath = "~/QuickForm/Style/";
        static string JQueryDependency = "~/Content/Scripts/JQuery/jquery-1.5.1.js"; // TODO: remove this

        
        public static void RegisterCssVirtualPath(string path)
        {
            QfDependencyExtension.CssVirtualPath = path;
        }

        public static void RegisterJQueryDependency(string path)
        {
            QfDependencyExtension.JQueryDependency = path;
        }
        
        // TODO: outfit this with Fluent stuff
        public static string MakeQfHtmlHead(this HtmlHelper html)
        {
            return
                html.Stylesheet(CssVirtualPath + "QfStyle.css").ToString() + Environment.NewLine +
                html.Stylesheet(CssVirtualPath + "QfButtons.css").ToString() + Environment.NewLine +
                html.Javascript(JQueryDependency).ToString() + Environment.NewLine;
        }
    }
}