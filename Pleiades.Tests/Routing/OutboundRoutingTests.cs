using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Gallio.Framework;
using MbUnit.Framework;
using Rhino.Mocks;
using Pleiades.Storefront.Tests.Helpers;
using Pleiades.Storefront.WebUI;
using Pleiades.Storefront.WebUI.Areas.Admin;
using Pleiades.Storefront.WebUI.Areas.StoreFront;
using Pleiades.Tests.Utilities.Web;


namespace Pleiades.Storefront.Tests.Routing
{
    [TestFixture]
    public class OutboundRoutingTests : RoutingTests
    {
        /// <summary>
        /// Basic Route Test - ignores Areas completely, granularly verifies the Route Data
        /// </summary>
        [Test]
        public void TestStoreFrontOutboundLinks()
        {
            string Url3 = GenerateUrlViaMocks(
                new  { area = "StoreFront", controller = "Products", action = "List", page = "1" }, new RouteData());
            string Url2 = GenerateUrlViaMocks(
                new { area = "StoreFront", controller = "Products", action = "List", category = "Watersports", page = "1" }, new RouteData());
            string Url = GenerateUrlViaMocks
                (new { controller = "Home", action = "List", value = "2" }, new RouteData());

            // TODO: Add Assertions
        }

        private string GenerateUrlViaMocks(object values, RouteData currentRouteData)
        {
            var httpcontext = HttpContextStubFactory.Make(AppRelativeCurrentExecutionFilePath: null);

            // Question: should I put the current Route in there?
            RequestContext requestcontext = new RequestContext(httpcontext, currentRouteData);

            return UrlHelper.GenerateUrl(null, null, null,
                    new RouteValueDictionary(values), routes, requestcontext, true);
        }
    }
}

