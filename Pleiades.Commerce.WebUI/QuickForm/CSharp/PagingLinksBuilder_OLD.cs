//using System;
//using System.Text;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Mvc.Html;
//using Pleiades.Web.Common.Paging;
//using PagedList;

//namespace Pleiades.Commerce.WebUI.Plumbing.UI.QuickForm
//{
//    /// <summary>
//    /// How to place this in a common assembly.. like AutoCalc.Web.Common
//    /// </summary>
//    public static class PagingLinksBuilderExtension
//    {
//        public const string CssSelectedLink = "selected";
//        public const string CssClass = "pagelink";

//        public static MvcHtmlString StandardPageLinks<T>(this HtmlHelper html, 
//                IPagedList<T> pagingInfo, Func<int, string> pageUrlGenerator)
//        {
//            StringBuilder result = new StringBuilder();

//            if (pagingInfo.PageCount <= 1)
//            {
//                return MvcHtmlString.Empty;
//            }

//            for (int pageNumber = 1; pageNumber <= pagingInfo.PageCount; pageNumber++)
//            {
//                string innerText = "Page " + pageNumber.ToString();

//                if (pagingInfo.PageNumber == pageNumber)
//                {
//                    var tag = new TagBuilder("span");
//                    tag.InnerHtml = innerText;
//                    result.AppendLine(tag.ToString());
//                }
//                else
//                {
//                    string href = pageUrlGenerator(pageNumber);
                    
//                    // Create the HTML tag
//                    TagBuilder tag = new TagBuilder("a");

//                    // Add the Url attribute
//                    tag.MergeAttribute("href", href);

//                    // Create the page link text
//                    tag.InnerHtml = innerText;
//                    tag.AddCssClass(CssClass);

//                    result.AppendLine(tag.ToString());
//                }
//            }

//            TagBuilder outerdiv = new TagBuilder("div");
//            outerdiv.AddCssClass("pager");
//            outerdiv.InnerHtml = result.ToString();

//            return MvcHtmlString.Create(outerdiv.ToString());
//        }
//    }
//}

