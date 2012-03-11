using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Pleiades.Web.Common.PagedModel;
using PagedList;

namespace Pleiades.Web.Common.MvcHelpers
{
    /// <summary>
    /// TODO: evaluate if this is still viable, usable, etc.
    /// 
    /// Paging Link Generators for AutoCalc.Web v1.0
    /// How to place this in a common assembly.. like AutoCalc.Web.Common
    /// </summary>
    public static class PageLinkGenerators
    {
        public const string CssSelectedLink = "selected";

        public static MvcHtmlString PageLinksGenerator<T>(this HtmlHelper html, 
                IPagedList<T> pagingInfo, Func<int, string> pageUrlGenerator)
        {
            var result = new StringBuilder();

            for (int pageNumber = 1; pageNumber <= pagingInfo.PageCount; pageNumber++)
            {
                string innerText = "Page " + pageNumber.ToString();

                if (pagingInfo.PageNumber == pageNumber)
                {
                    var tag = new TagBuilder("span");
                    tag.InnerHtml = innerText;
                    result.AppendLine(tag.ToString());
                }
                else
                {
                    string href = pageUrlGenerator(pageNumber);
                    
                    // Create the HTML tag
                    TagBuilder tag = new TagBuilder("a");

                    // Add the Url attribute
                    tag.MergeAttribute("href", href);

                    // Create the page link text
                    tag.InnerHtml = innerText;

                    // Selected Class
                    if (pageNumber == pagingInfo.PageNumber)
                    {
                        tag.AddCssClass(CssSelectedLink);
                    }

                    result.AppendLine(tag.ToString());
                }
            }

            TagBuilder outerdiv = new TagBuilder("div");
            outerdiv.AddCssClass("pager");
            outerdiv.InnerHtml = result.ToString();

            return MvcHtmlString.Create(outerdiv.ToString());
        }
    }
}

