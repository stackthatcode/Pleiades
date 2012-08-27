﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Pleiades.Helpers;

namespace Pleiades.Web.MvcHelpers
{
    /// <summary>
    /// Generic Tag generator
    /// </summary>
    public static class TagIfNotEmptyExtension
    {
        public static MvcHtmlString TagIfNotEmpty(
            this HtmlHelper html, object source, string tagname, string @class = "", string style = "")
        {
            if (source == null || source.ToString().Trim() == "")
            {
                return MvcHtmlString.Create("");
            }

            var tag = new TagBuilder(tagname);

            if (@class != "")
            {
                tag.AddCssClass(@class);
            }

            if (style != "")
            {
                tag.MergeAttribute("style", style);
            }
            
            tag.InnerHtml += source.ToString();
            
            return MvcHtmlString.Create(tag.ToString());
        }
    }
}
