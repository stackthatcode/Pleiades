using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Pleiades.Commerce.WebUI.Plumbing.UI.HtmlHelpers
{
    public static class StandardRadioButtonExtension
    {
        public const string DivCssClass = "stdradiodiv";
        public const string LabelCssClass = "stdradiolabel";

        public static MvcHtmlString StandardRadioButton<TModel, TProperty>(this HtmlHelper<TModel> helper, 
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

    }
}

