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
        // TODO: provide hooks to enable folks to choose how to render validation            

        public const string QfDivContainerClass = "qf-container";
        public const string QfRadioLabelClass = "qf-radiolabel";
        public const string ErrorMessage = "ErrorMessage";


        public static MvcHtmlString QfTextEditorFor<TModel, TProperty>(
            this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> property, int maxlength = 50, string style = "")
        {
            var textboxHtml = html.TextBoxFor(property, new { style = style, maxlength = maxlength }).ToHtmlString();
            return WrapInAContainerDiv(html, property, textboxHtml);
        }

        public static MvcHtmlString QfPasswordFor<TModel, TProperty>(
            this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, int maxlength = 25, string style = "")
        {
            var editorHtml = html.PasswordFor(expression, new { style = style, maxlength = maxlength }).ToHtmlString();
            return WrapInAContainerDiv(html, expression, editorHtml);
        }

        public static MvcHtmlString QfDropDownListFor<TModel, TProperty>(
                this HtmlHelper<TModel> htmlHelper,
                Expression<Func<TModel, TProperty>> expression,
                IEnumerable<SelectListItem> selectList,
                object htmlAttributes = null)
        {
            var editorHtml = htmlHelper.DropDownListFor(expression, selectList, htmlAttributes).ToHtmlString();
            return WrapInAContainerDiv(htmlHelper, expression, editorHtml);
        }
        
        public static MvcHtmlString WrapInAContainerDiv<TModel, TProperty>(
            this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string editorHtml)
        {
            var container = new TagBuilder("div");
            container.AddCssClass(QfDivContainerClass);
            container.InnerHtml += editorHtml;
            container.InnerHtml += html.ValidationMessageFor(expression);

            return MvcHtmlString.Create(container.ToString());
        }

        public static MvcHtmlString QfLabelFor<TModel, TProperty>(
                this HtmlHelper<TModel> htmlHelper,
                Expression<Func<TModel, TProperty>> expression,
                string overrideText = null)
        {
            var labelHtml = (overrideText == null) ?
                htmlHelper.LabelFor<TModel, TProperty>(expression).ToHtmlString() :
                htmlHelper.LabelFor<TModel, TProperty>(expression, overrideText).ToHtmlString();

            var container = new TagBuilder("div");
            container.AddCssClass(QfDivContainerClass);
            container.InnerHtml += htmlHelper.ValidationMessageFor(expression, "* ", new { style = "font:bold;" });
            container.InnerHtml += labelHtml;

            return MvcHtmlString.Create(container.ToString());
        }

        public static MvcHtmlString QfRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> helper,
                Expression<Func<TModel, TProperty>> expression, string value, string label, string style = "")
        {
            var member = (MemberExpression)expression.Body;
            var memberName = member.Member.Name;
            var radio_id = memberName + "_" + value;

            var parentDiv = new TagBuilder("div");
            parentDiv.AddCssClass(QfDivContainerClass);
            parentDiv.MergeAttribute("style", style);

            var labelTag = new TagBuilder("label");
            labelTag.MergeAttribute("for", radio_id);
            labelTag.AddCssClass(QfRadioLabelClass);

            labelTag.InnerHtml = label;
            parentDiv.InnerHtml += labelTag.ToString();
            parentDiv.InnerHtml += helper.RadioButtonFor(expression, value, new { id = radio_id });

            return MvcHtmlString.Create(parentDiv.ToString());
        }

        public static MvcHtmlString QfValidationSummary<TModel>(this HtmlHelper<TModel> helper, string style = "")
        {
            var parentDiv = new TagBuilder("div");
            parentDiv.AddCssClass(QfDivContainerClass);
            parentDiv.MergeAttribute("style", style);
            parentDiv.InnerHtml = helper.ValidationSummary().ToString();
            return MvcHtmlString.Create(parentDiv.ToString());
        }
    }
}
