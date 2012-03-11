using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Pleiades.Commerce.WebUI.Plumbing.UI.HtmlHelpers
{
    public static class StandardTextBoxExtension
    {
        public const string CssClass = "stdtextbox";

        public static MvcHtmlString StandardTextBoxFor<TModel, TProperty>(
                this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, int maxlength, string style = "")
        {
            var textboxHtml = html.TextBoxFor(
                    expression, new { @class = CssClass, style = style, maxlength = maxlength }).ToHtmlString();

            return HelperFunction(html, expression, maxlength, textboxHtml);
        }

        public static MvcHtmlString StandardPasswordFor<TModel, TProperty>(
                this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, int maxlength, string style = "")
        {
            var textboxHtml = html.PasswordFor(
                    expression, new { @class = CssClass, style = style, maxlength = maxlength }).ToHtmlString();

            return HelperFunction(html, expression, maxlength, textboxHtml);
        }

        private static MvcHtmlString HelperFunction<TModel, TProperty>(
                HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, int maxlength, string textboxHtml)
        {
            // Containing Div
            var container = new TagBuilder("div");
            container.InnerHtml += textboxHtml;

            // Following-clearing Div
            var clearDiv = new TagBuilder("div");
            clearDiv.AddCssClass("clr");
            container.InnerHtml += clearDiv.ToString();

            // Validation Message
            container.InnerHtml += html.ValidationMessageFor(expression);

            return MvcHtmlString.Create(container.ToString());
        }
    }
}
