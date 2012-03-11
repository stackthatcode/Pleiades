using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Pleiades.Web.Common.MvcHelpers
{
    /// <summary>
    /// Container object for passing around/processing/rendering Navigation Links
    /// </summary>
    public class NavigationLink
    {
        public string Text { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
        public bool Selected { get; set; }
    }
}
