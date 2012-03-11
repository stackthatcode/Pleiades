using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Pleiades.Commerce.WebUI.Plumbing.UI.HtmlHelpers
{
    public enum StandardButton
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

    public static class StandardButtonExtension
    {
        public static Dictionary<StandardButton, string> StandardButtonToCssClass = 
            new Dictionary<StandardButton, string>()
                {
                    { StandardButton.Edit, "editbutton" },
                    { StandardButton.Cancel, "cancelbutton" },
                    { StandardButton.Reset, "resetbutton" },
                    { StandardButton.Delete, "deletebutton" },
                    { StandardButton.Save, "savebutton" },
                    { StandardButton.Yes, "yesbutton" },
                    { StandardButton.No, "nobutton" },
                    { StandardButton.Unlock, "unlockbutton" },
                    { StandardButton.Change, "changebutton" },
                    { StandardButton.Ok, "okbutton" },
                    { StandardButton.Done, "donebutton" },
                    { StandardButton.Back, "backbutton" },
                };

        public static MvcHtmlString StandardActionButton(this HtmlHelper helper,
                StandardButton button, string actionName, string style = "", object routeValues = null)
        {
            return helper.ActionLink(
                actionName,
                actionName,
                routeValues,
                new { @class = StandardButtonToCssClass[button], style = style });
        }

        public static MvcHtmlString StandardSubmitButton(this HtmlHelper helper,
                StandardButton button, string value, string style = "", object routeValues = null)
        {
            TagBuilder tag = new TagBuilder("input");
            tag.MergeAttribute("type", "submit");
            tag.MergeAttribute("value", value);
            tag.MergeAttribute("style", style);
            tag.AddCssClass(StandardButtonToCssClass[button]);
            return MvcHtmlString.Create(tag.ToString());
        }
    }
}

