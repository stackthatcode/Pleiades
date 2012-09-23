using System;
using System.Web.Mvc;

namespace Commerce.WebUI.QuickForm.CSharp
{
    public static class QfClearDivExtensions
    {
        public static MvcHtmlString QfClearDiv<TModel>(this HtmlHelper<TModel> html)
        {
            var clearDiv = new TagBuilder("div");
            clearDiv.AddCssClass("qf-clr");
            return MvcHtmlString.Create(clearDiv.ToString());   // Is this necessary...?
        }
    }
}