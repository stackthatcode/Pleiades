using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Pleiades.Framework.Data;

namespace Pleiades.Web.MvcHelpers
{
    /// <summary>
    /// </summary>
    public static class PageLinkGenerators
    {
        public const string CssSelectedLink = "selected";

        public static MvcHtmlString PageLinksGenerator<T>(this HtmlHelper html, 
                IQueryable<T> data, Func<int, string> pageUrlGenerator)
        {
            var result = new StringBuilder();

            for (int pageNumber = 1; pageNumber <= data.MaxPageNumber<T>(); pageNumber++)
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

