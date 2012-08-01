using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Commerce.WebUI;
using Pleiades.Framework.TestHelpers;
using Pleiades.Framework.TestHelpers.Web;

namespace Pleiades.Commerce.WebUI.UnitTests.Routing
{
    [TestFixture]
    public class ProductRouteTests : RoutingTestBase
    {
        [Test]
        public void VerifyListAllCategoriesByPageRoute()
        {
            // Inbound 
            var routeData2 =
                RoutingHelper.VerifyInboundRoute(
                    RouteTable.Routes,
                    inboundUrl: "~/Public/Page1",
                    expectedRouteValues: new { controller = "Products", category = (string)null, action = "List", page = "1" },
                    expectedDataTokens: new { area = "Public" });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                RouteTable.Routes,
                new { area = "Public", controller = "Products", action = "List", page = "1" });

            url.ShouldEqual("/Public/Page1");
        }

        [Test]
        public void VerifyListByCategoryRoute()
        {
            // Inbound 
            var routeData2 = 
                RoutingHelper.VerifyInboundRoute(
                    RouteTable.Routes,
                    inboundUrl: "~/Public/Motorcycles",
                    expectedRouteValues: new { controller = "Products", action = "List", category = "Motorcycles" },
                    expectedDataTokens: new { area = "Public" });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                RouteTable.Routes,
                new { area = "Public", controller = "Products", action = "List", category = "Motorcycles" });

            url.ShouldEqual("/Public/Motorcycles");
        }

        [Test]
        public void VerifyListByCategoryAndByPageRoute()
        {
            // Inbound 
            var routeData2 = 
                RoutingHelper.VerifyInboundRoute(
                    RouteTable.Routes,
                    inboundUrl: "~/Public/Motorcycles/Page3",
                    expectedRouteValues: new { controller = "Products", action = "List", category = "Motorcycles", page = "3" },
                    expectedDataTokens: new { area = "Public" });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                RouteTable.Routes,
                new { area = "Public", controller = "Products", action = "List", category = "Motorcycles", page = "3" });

            url.ShouldEqual("/Public/Motorcycles/Page3");
        }

        [Test]
        public void VerifyProductsGetImageByProductIdRoute()
        {
            // Inbound 
            var routeData2 = 
                RoutingHelper.VerifyInboundRoute(
                    RouteTable.Routes,
                    inboundUrl: "~/Public/Products/GetImage/7",
                    expectedRouteValues: new { controller = "Products", action = "GetImage", productid = "7" },
                    expectedDataTokens: new { area = "Public" });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                RouteTable.Routes,
                new { area = "Public", controller = "Products", action = "GetImage", productid = "7" });

            url.ShouldEqual("/Public/Products/GetImage/7");
        }

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
