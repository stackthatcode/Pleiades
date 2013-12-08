using System.Web.Routing;
using Commerce.UnitTests.Routing;
using NUnit.Framework;
using Pleiades.TestHelpers.Web;

namespace Commerce.Web.UnitTests.Routing
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
                    inboundUrl: "~/",
                    expectedRouteValues: new { controller = "Home", action = "Index" });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                RouteTable.Routes,
                new { controller = "Home", action = "Index" });
            url.ShouldEqual("/");
        }

        [Test]
        public void VerifyControllerAndActionAndId()
        {
            // Inbound 
            var routeData2 = RoutingHelper.VerifyInboundRoute(
                RouteTable.Routes,
                inboundUrl: "~/TestController/TestAction/3",
                expectedRouteValues: new { controller = "TestController", action = "TestAction", id = "3" });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                RouteTable.Routes,
                new { controller = "TestController", action = "TestAction", id = "3" });
            url.ShouldEqual("/TestController/TestAction/3");
        }

        [Test]
        public void VerifyControllerAndActionRoute()
        {
            // Inbound 
            var routeData2 = RoutingHelper.VerifyInboundRoute(
                RouteTable.Routes,
                inboundUrl: "~/TestController/TestAction",
                expectedRouteValues: new { controller = "TestController", action = "TestAction" });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                RouteTable.Routes,
                new { controller = "TestController", action = "TestAction" });
            url.ShouldEqual("/TestController/TestAction");
        }
    }
}
