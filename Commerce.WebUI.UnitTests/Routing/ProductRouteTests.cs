using System.Web.Routing;
using NUnit.Framework;
using Pleiades.TestHelpers;
using Pleiades.TestHelpers.Web;

namespace Commerce.Web.UnitTests.Routing
{
    [TestFixture]
    public class ProductRouteTests : RoutingTestBase
    {
       
        [Test]
        public void VerifyProductsAbstractControllerAndActionRoute()
        {
            // Inbound 
            var routeData2 = 
                RoutingHelper.VerifyInboundRoute(
                RouteTable.Routes,
                inboundUrl: "~/Public/Products/Discount",
                expectedRouteValues: new { controller = "Products", action = "Discount" },
                expectedDataTokens: new { area = "Public" });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                RouteTable.Routes,
                new { area = "Public", controller = "Products", action = "Discount" });

            url.ShouldEqual("/Public/Products/Discount");
        }
    }
}
