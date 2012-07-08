using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Commerce.WebUI;
using Pleiades.Framework.TestHelpers;
using Pleiades.Framework.TestHelpers.Web;

namespace Pleiades.Commerce.WebUI.TestsRouting
{
    [TestFixture]
    public class ProductRoutes : RoutingTestBase
    {
        [Test]
        public void RootListAllCategoriesNOPage()
        {
            // Inbound 
            var routeData2 = RoutingHelper.TestInboundRoute(
                this.Routes,
                "http://google.com",
                "~/",
                new { controller = "Products", category = (string)null, action = "List" },
                new { });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                this.Routes,
                new { controller = "Products", action = "List", category = (string)null });
            url.ShouldEqual("/");
        }

        [Test]
        public void ListAllCategoriesByPage()
        {
            // Inbound 
            var routeData2 = RoutingHelper.TestInboundRoute(
                this.Routes,
                "http://google.com",
                "~/Page1",
                new { controller = "Products", category = (string)null, action = "List", page = "1" },
                new { });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                this.Routes,
                new { controller = "Products", action = "List", page = "1" });

            url.ShouldEqual("/Page1");
        }

        [Test]
        public void ListByCategory()
        {
            // Inbound 
            var routeData2 = RoutingHelper.TestInboundRoute(
                this.Routes,
                "http://google.com",
                "~/Motorcycles",
                new { controller = "Products", action = "List", category = "Motorcycles" },
                new { });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                this.Routes,
                new { controller = "Products", action = "List", category = "Motorcycles" });

            url.ShouldEqual("/Motorcycles");
        }

        [Test]
        public void ListByCategoryAndByPage()
        {
            // Inbound 
            var routeData2 = RoutingHelper.TestInboundRoute(
                this.Routes,
                "http://google.com",
                "~/Motorcycles/Page3",
                new { controller = "Products", action = "List", category = "Motorcycles", page = "3" },
                new { });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                this.Routes,
                new { controller = "Products", action = "List", category = "Motorcycles", page = "3" });

            url.ShouldEqual("/Motorcycles/Page3");
        }

        [Test]
        public void ProductsGetImageByProductId()
        {
            // Inbound 
            var routeData2 = RoutingHelper.TestInboundRoute(
                this.Routes,
                "http://google.com",
                "~/Products/GetImage/7",
                new { controller = "Products", action = "GetImage", productid = "7" },
                new { });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                this.Routes,
                new { controller = "Products", action = "GetImage", productid = "7" });

            url.ShouldEqual("/Products/GetImage/7");
        }

        [Test]
        public void ProductsAbstractControllerANDAction()
        {
            // Inbound 
            var routeData2 = RoutingHelper.TestInboundRoute(
                this.Routes,
                "http://google.com",
                "~/Products/List",
                new { controller = "Products", action = "List" },
                new { });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                this.Routes,
                new { controller = "Products", action = "List" });

            url.ShouldEqual("/");
        }
    }
}
