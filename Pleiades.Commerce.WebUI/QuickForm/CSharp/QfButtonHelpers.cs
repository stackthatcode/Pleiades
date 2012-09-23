using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Commerce.WebUI.QuickForm.CSharp
{
    public enum QfButtonId
    {
        Edit,
        Cancel,
        Reset,
        Delete,
        Save,
        Yes,
        No,
        Unlock,
        Change,
        Ok,
        Done,
        Back,
    }

    public static class QfButtonHelpers
    {
        private readonly static Dictionary<QfButtonId, string> ButtonIdToCssClass = 
                new Dictionary<QfButtonId, string>()
                {
                    { QfButtonId.Edit, "qf-editbutton" },
                    { QfButtonId.Cancel, "qf-cancelbutton" },
                    { QfButtonId.Reset, "qf-resetbutton" },
                    { QfButtonId.Delete, "qf-deletebutton" },
                    { QfButtonId.Save, "qf-savebutton" },
                    { QfButtonId.Yes, "qf-yesbutton" },
                    { QfButtonId.No, "qf-nobutton" },
                    { QfButtonId.Unlock, "qf-unlockbutton" },
                    { QfButtonId.Change, "qf-changebutton" },
                    { QfButtonId.Ok, "qf-okbutton" },
                    { QfButtonId.Done, "qf-donebutton" },
                    { QfButtonId.Back, "qf-backbutton" },
                };

        public static MvcHtmlString QfActionButton(this HtmlHelper helper,
                QfButtonId button, string actionName, string style = null, object route = null)
        {
            return helper.ActionLink(
                actionName, actionName, route, new { @class = ButtonIdToCssClass[button], style = style });
        }

        public static MvcHtmlString QfRouteButton(this HtmlHelper helper,
                QfButtonId button, string alternateText, RouteValueDictionary route, string style = null)
        {
            return helper.RouteLink(alternateText, route,
                new RouteValueDictionary(new { @class = ButtonIdToCssClass[button], style = style }));
        }

        public static MvcHtmlString QfSubmitButton(
                this HtmlHelper helper, QfButtonId button, string value, string style = null)
        {
            var tag = new TagBuilder("input");
            tag.MergeAttribute("type", "submit");
            tag.MergeAttribute("value", value);
            tag.MergeAttribute("style", style);
            tag.AddCssClass(ButtonIdToCssClass[button]);
            return MvcHtmlString.Create(tag.ToString());
        }
    }
}