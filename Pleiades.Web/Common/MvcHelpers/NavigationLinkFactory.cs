using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Pleiades.Utilities.General;

namespace Pleiades.Web.Common.MvcHelpers
{
    /// <summary>
    /// TODO: abstract the setting of various Link Route Values
    /// </summary>
    public class NavigationLinkFactory
    {
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Page { get; set; }

        public NavigationLink Create(string linkText, bool linkSelected, params string[] linkRouting)
        {
            var linkRouteValues = new RouteValueDictionary();

            if (Area != null) linkRouteValues["area"] = Area;
            if (Controller != null) linkRouteValues["controller"] = Controller;
            if (Action != null) linkRouteValues["action"] = Action;
            if (Page != null) linkRouteValues["page"] = Page;

            // Could've done this:
            //            if (Page != null) linkRouteValues["page"] = Page.ToInt32();
            // But Routing expects type-safety.  Sorry!
 
            for (int i = 0; i < linkRouting.Length; i += 2)
            {
                string routeParameterName = linkRouting[i];
                string routeParameterValue = linkRouting[i + 1];
                linkRouteValues[routeParameterName] = routeParameterValue;
            }

            return new NavigationLink
            {
                RouteValues = linkRouteValues,
                Selected = linkSelected,
                Text = linkText,
            };
        }
    }
}
