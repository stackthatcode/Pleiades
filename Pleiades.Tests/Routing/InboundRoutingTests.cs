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
using Pleiades.Tests.Utilities.Web;

namespace Pleiades.Storefront.Tests.Routing
{
    [TestFixture]
    public class InboundRoutingTests : RoutingTests
    {
        /// <summary>
        /// Basic Route Test - ignores Areas completely, granularly verifies the Route Data
        /// </summary>
        [Test]
        public void TestDefaultAreaRoute()
        {
            // This passes
            var mockcontext = HttpContextStubFactory.Make(
                AuthenticatedName: "Bob", IsAuthenticated: true, AppRelativeCurrentExecutionFilePath: "~/Test333/789");

            // GetRouteData will return a non-null RouteData reference if the Url scores a match
            var routeData1 = routes.GetRouteData(mockcontext);
            Assert.IsNotNull(routeData1);
            Assert.AreEqual<string>(routeData1.Values["controller"].ToString(), "Home");
            Assert.AreEqual<string>(routeData1.Values["action"].ToString(), "List");
            Assert.AreEqual<string>(routeData1.Values["value"].ToString(), "789"); 
        }

        /// <summary>
        /// Tests the variable length path in the Proving Ground Area Registration
        /// </summary>
        [Test]
        public void TestBlankRouteDefaultRoute()
        {
            var routeData2 = RoutingHelper.TestInboundRoute(
                routes,
                "http://localhost:6717/",
                "~/",
                new { controller = "Products", action = "List" },
                new { area = "StoreFront" });
        }

        [Test]
        public void TestAnotherRoute()
        {
            var routeData2 = RoutingHelper.TestInboundRoute(
                routes,
                "http://localhost:6717/",
                "~/StoreFront/Hiking/Page3",
                new { controller = "Products", action = "List", page = "3" },
                new { area = "StoreFront" });
        }
    }
}
