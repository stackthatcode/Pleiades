using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Pleiades.Web.MvcHelpers
{
    /// <summary>
    /// Convenience methods banging out Label + Editor combinations
    /// 
    /// TODO: enable custom Editor injection via the parameters (or otherwise?)
    /// TODO: enable the custom help text
    /// </summary>
    public static class FormLineExtensions
    {
        /// <summary>
        /// Creates combination of HtmlHelper-spawned Label and DropDownList wrapped in a div tag container
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="modelProperty">Specifies which Model Property to make an EditorFor</param>
        /// <param name="templateName">Overrideable option for Editor template</param>
        /// <param name="selectList">Sequence of SelectListItems</param>
        /// <param name="labelText">Label Text</param>
        /// <param name="customHelpText">TEMPORARILY DISABLED</param>
        /// <param name="containerHtmlAttributes">Additional Html Attributes for </param>
        /// <returns>MvcHtmlString command object</returns>
        public static MvcHtmlString FormLineDropDownListFor<TModel, TProperty>(
                this HtmlHelper<TModel> htmlHelper, 
                Expression<Func<TModel, TProperty>> modelProperty, 
                IEnumerable<SelectListItem> selectList, 
                string labelText = null, 
                string customHelpText = null, 
                object containerHtmlAttributes = null)
        {
            // Create the Label HTML
            var labelHtml = htmlHelper.LabelFor(modelProperty, labelText).ToString();

            // Create the Editor HTML
            var editorHtml =
                htmlHelper.DropDownListFor(modelProperty, selectList, containerHtmlAttributes).ToString() +
                htmlHelper.ValidationMessageFor(modelProperty);


            return FormLine(labelHtml, editorHtml);
        }

        /// <summary>
        /// Creates combination of HtmlHelper-spawned Label and Editor wrapped in a div tag container
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="modelProperty">Specifies which Model Property to make an EditorFor</param>
        /// <param name="templateName">Overrideable option for Editor template</param>
        /// <param name="labelText">Label Text</param>
        /// <param name="customHelpText">TEMPORARILY DISABLED</param>
        /// <param name="containerHtmlAttributes">Additional Html Attributes for </param>
        /// <returns>MvcHtmlString command object</returns>
        public static MvcHtmlString FormLineEditorFor<TModel, TProperty>(
                this HtmlHelper<TModel> htmlHelper, 
                Expression<Func<TModel, TProperty>> modelProperty, 
                string templateName = null, 
                string labelText = null,
                string customHelpText = null, 
                object containerHtmlAttributes = null)
        {
            // Create the default Label
            var labelHtml = htmlHelper.LabelFor(modelProperty, labelText).ToString();

            // Create the default Editor
            var editorHtml =
                htmlHelper.EditorFor(modelProperty, templateName, containerHtmlAttributes).ToString() +
                htmlHelper.ValidationMessageFor(modelProperty);

            return FormLine(labelHtml, editorHtml);
        }

        /// <summary>
        // Worker method assembles Label and Field Content (html)
        /// </summary>
        private static MvcHtmlString FormLine(
                string labelContent, string fieldContent, object containerHtmlAttributes = null)
        {
            // Create the Label Div
            var editorLabel = new TagBuilder("div");
            editorLabel.AddCssClass("editor-label");
            editorLabel.InnerHtml += labelContent;

            // Create the Editor Field Div
            var editorField = new TagBuilder("div");
            editorField.AddCssClass("editor-field");
            editorField.InnerHtml += fieldContent;

            // Create the Container Div - which holds Label + Editor
            var container = new TagBuilder("div");
            if (containerHtmlAttributes != null)
            {
                container.MergeAttributes(new RouteValueDictionary(containerHtmlAttributes));
            }

            container.AddCssClass("form-line");
            container.InnerHtml += editorLabel;
            container.InnerHtml += editorField;

            // Return MvcHtmlString processed html
            return MvcHtmlString.Create(container.ToString());
        }



        /*
        // TODO: make it so this shows tool tips over label and control
        // Get some MvcJs or JQuery code to do this...
        public static MvcHtmlString HelpTextFor<TModel, TProperty>(
                this HtmlHelper<TModel> helper, 
                Expression<Func<TModel, TProperty>> expression, 
                string customText = null)
        {
            // Can do all sorts of things here -- eg: reflect over attributes and add hints, etc...
        } 
         */
    }
}
