using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Commerce.WebUI.QuickForm.CSharp
{
    public static class QfTextEditorExtensions
    {
        public const string DivCssClass = "qfradiodiv";
        public const string ErrorMessage = "ErrorMessage";
        public const string ContainerCssClass = "qf-editor";


        public static MvcHtmlString QfTextEditorFor<TModel, TProperty>(
            this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> property, int maxlength = 50, string style = "")
        {
            var textboxHtml = html.TextBoxFor(
                    property, new { style = style, maxlength = maxlength }).ToHtmlString();

            return HelperFunction(html, property, textboxHtml);
        }

        public static MvcHtmlString QfPasswordFor<TModel, TProperty>(
            this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, int maxlength = 25, string style = "")
        {
            var editorHtml = html.PasswordFor(
                    expression, new { style = style, maxlength = maxlength }).ToHtmlString();

            return HelperFunction(html, expression, editorHtml);
        }

        public static MvcHtmlString QfDropDownListFor<TModel, TProperty>(
                this HtmlHelper<TModel> htmlHelper,
                Expression<Func<TModel, TProperty>> property,
                IEnumerable<SelectListItem> selectList,
                object htmlAttributes = null)
        {

            // Create the Container Div - which holds Label + Editor
            var container = new TagBuilder("div");
            if (htmlAttributes != null)
            {
                container.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }

            // Create the Editor HTML
            var editorHtml =
                htmlHelper.DropDownListFor(property, selectList, htmlAttributes).ToString() +
                htmlHelper.ValidationMessageFor(property);

            return HelperFunction(htmlHelper, property, editorHtml);
        }

        public static MvcHtmlString QfRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
                Expression<Func<TModel, TProperty>> expression, string value, string label, string style = "")
        {
            var member = (MemberExpression)expression.Body;
            var memberName = member.Member.Name;
            var radio_id = memberName + "_" + value;

            var parentDiv = new TagBuilder("div");
            parentDiv.AddCssClass(DivCssClass);
            parentDiv.MergeAttribute("style", style);

            var labelTag = new TagBuilder("label");
            labelTag.MergeAttribute("for", radio_id);
            labelTag.AddCssClass(LabelCssClass);

            labelTag.InnerHtml = label;
            parentDiv.InnerHtml += labelTag.ToString();
            parentDiv.InnerHtml += helper.RadioButtonFor(expression, value, new { id = radio_id });

            return MvcHtmlString.Create(parentDiv.ToString());
        }

        // TODO: add QfLabelFor



        private static MvcHtmlString HelperFunction<TModel, TProperty>(
            this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string editorHtml)
        {
            // Containing Div
            var container = new TagBuilder("div");
            container.AddCssClass(QfDependencyExtension.ContainerCssClass);
            container.InnerHtml += editorHtml;
            container.InnerHtml += html.ValidationMessageFor(expression);

            // Following-clearing Div -- UPDATE: provide hooks to enable folks to choose how to render validation            
            // Validation Message

            return MvcHtmlString.Create(container.ToString());
        }
    }
}
