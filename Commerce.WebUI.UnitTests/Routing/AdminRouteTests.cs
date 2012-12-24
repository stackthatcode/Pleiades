using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;
using Commerce.WebUI;
using Pleiades.TestHelpers;
using Pleiades.TestHelpers.Web;

namespace Commerce.WebUI.UnitTests.Routing
{
    [TestFixture]
    public class AdminRouteTests : RoutingTestBase
    {
        [Test]
        public void VerifyHomeRoute()
        {
            // Inbound 
            var routeData2 = 
                RoutingHelper.VerifyInboundRoute(
                    RouteTable.Routes,
                    inboundUrl: "~/Admin",
                    expectedRouteValues: new { controller = "Home", action = "Index" },
                    expectedDataTokens: new { area = "Admin" });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                RouteTable.Routes,
                new { area = "Admin", controller = "Home", action = "Index" });
            url.ShouldEqual("/Admin");
        }

        [Test]
        public void VerifyControllerAndActionAndId()
        {
            // Inbound 
            var routeData2 = RoutingHelper.VerifyInboundRoute(
                RouteTable.Routes,
                inboundUrl: "~/Admin/TestController/TestAction/3",
                expectedRouteValues: new { controller = "TestController", action = "TestAction", id = "3" },
                expectedDataTokens: new { area = "Admin" });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                RouteTable.Routes,
                new { area = "Admin", controller = "TestController", action = "TestAction", id = "3" });
            url.ShouldEqual("/Admin/TestController/TestAction/3");
        }

        [Test]
        public void VerifyControllerAndActionRoute()
        {
            // Inbound 
            var routeData2 = RoutingHelper.VerifyInboundRoute(
                RouteTable.Routes,
                inboundUrl: "~/Admin/TestController/TestAction",
                expectedRouteValues: new { controller = "TestController", action = "TestAction" },
                expectedDataTokens: new { area = "Admin" });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                RouteTable.Routes,
                new { area = "Admin", controller = "TestController", action = "TestAction" });
            url.ShouldEqual("/Admin/TestController/TestAction");
        }
    }
}
