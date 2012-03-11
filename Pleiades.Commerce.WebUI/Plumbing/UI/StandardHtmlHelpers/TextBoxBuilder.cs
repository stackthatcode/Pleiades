using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Pleiades.Commerce.WebUI.Plumbing.UI.StandardHtmlHelpers
{
    public static class TextBoxBuilderExtensions
    {
        public const string CssClass = "stdtextbox";

        /// <summary>
        /// Creates Standard Text Boxes
        /// 
        /// NOTE: is coupled to the Pleiades.CSS StyleSheet
        /// </summary>
        public static MvcHtmlString 
            StandardTextBoxFor<TModel, TProperty>(
                this HtmlHelper<TModel> html, 
                Expression<Func<TModel, TProperty>> propertyExpression, 
                int maxlength, 
                string style = "")
        {
            var textboxHtml = 
                html.TextBoxFor(
                    propertyExpression, 
                    new { @class = CssClass, style = style, maxlength = maxlength }).ToHtmlString();

            return HelperFunction(html, propertyExpression, maxlength, textboxHtml);
        }

        public static MvcHtmlString 
            StandardPasswordFor<TModel, TProperty>(
                this HtmlHelper<TModel> html, 
                Expression<Func<TModel, TProperty>> expression, 
                int maxlength, 
                string style = "")
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
