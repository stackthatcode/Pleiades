using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using HtmlAgilityPack;
using MbUnit.Framework;
using Pleiades.Storefront.Tests.Helpers;
using Pleiades.Web.HtmlHelpers;
using Pleiades.Web.Models;
using Pleiades.Tests.Utilities.General;

namespace Pleiades.Storefront.Tests
{
    [TestFixture]
    public class PagingTests
    {
        private List<object> GenerateTestList()
        {
            List<object> items = new List<object>();
            int counter = 0;
            while (counter++ < 28)
                items.Add(new object());
            return items;
        }

        [Test]
        public void TestHtmlOutputFromStandardPageLink()
        {
            PagedModel<object> pagedModel = new PagedModel<object>(GenerateTestList());
            pagedModel.CurrentPageNumber = 2;
            pagedModel.ItemsPerPage = 10;

            // This is faking what will be pass through routing system
            Func<int, string> pageUrl = (PageNum) => { return "Page" + PageNum.ToString(); };
            HtmlHelper html = null;
            MvcHtmlString result = html.StandardPageLinks(pagedModel, pageUrl);

            // *THIS* is how you test your HTML, son!
            HtmlAgilityPack.HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(result.ToString());
            
            var links = doc.DocumentNode.SelectNodes("//a").ToList();

            links[0].GetAttributeValue("href", "").ShouldEqual("Page1");
            links[0].GetAttributeValue("class", "").ShouldEqual("");
            links[0].InnerHtml.ShouldEqual("Page 1");

            links[1].GetAttributeValue("href", "").ShouldEqual("Page2");
            links[1].GetAttributeValue("class", "").ShouldEqual("selected");
            links[1].InnerHtml.ShouldEqual("Page 2");

            links[2].GetAttributeValue("href", "").ShouldEqual("Page3");
            links[2].GetAttributeValue("class", "").ShouldEqual("");
            links[2].InnerHtml.ShouldEqual("Page 3");
        }
    }
}
