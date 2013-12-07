using System.Web.Routing;
using NUnit.Framework;
using Pleiades.TestHelpers.Web;

namespace Commerce.UnitTests.Routing
{
    [Ignore("ArtOfGroundFighting.com Routes")]
    [TestFixture]
    public class ProductRouteTests : RoutingTestBase
    {
       
        [Test]
        public void Verify_Hit_on_ProductController_With_Id()
        {
            // Inbound 
            var routeData2 = 
                RoutingHelper.VerifyInboundRoute(
                RouteTable.Routes,
                inboundUrl: "~/Products/3",
                expectedRouteValues: new { controller = "Products", action = "action-with-id" });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                RouteTable.Routes,
                new {controller = "Products", id = 3 });

            url.ShouldEqual("/Products/3");
        }

        // Wierd but True stuff about Routes

        // NOTE #1: there are no Data Tokens on the default route, because it's not in the Area 
        // ... it just directs traffic stuff to the Products Controller.  Same concept as Note #2.

        // UPDATE: MVC was unable to find the Views

        // NOTE #2: when generating an Outbound URL, we don't use Data Tokens, since we don't know which Route is
        // ... going to match our Route Data.

        // NOTE #3: when parsing an Inbound URL, a Data Token will be in the Route Data of the matching Route

        [Test]
        public void Verify_That_Slash_Returns_DefaultRoute()
        {
            // Inbound-only
            var routeData2 =
                RoutingHelper.VerifyInboundRoute(
                    RouteTable.Routes,
                    inboundUrl: "~/",
                    expectedRouteValues:
                        new { controller = "Page", action = "Index", category = (string)null });
        }
    }
}
