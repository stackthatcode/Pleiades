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
using Pleiades.Commerce.WebUI;
using Pleiades.Utilities.TestHelpers.General;
using Pleiades.Utilities.TestHelpers.Web;

namespace Pleiades.Commerce.WebUI.TestsRouting
{
    [TestFixture]
    public class AdminRoutes : RoutingTestBase
    {
        [Test]
        public void DefaultRoute()
        {
            // Inbound 
            var routeData2 = RoutingHelper.TestInboundRoute(
                this.Routes,
                "http://testurl.com",
                "~/Admin",
                new { controller = "Home", action = "Index" },
                new { area = "Admin" });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                this.Routes,
                new { area = "Admin", controller = "Home", action = "Index" });
            url.ShouldEqual("/Admin");
        }

        [Test]
        public void ControllerActionAndId()
        {
            // Inbound 
            var routeData2 = RoutingHelper.TestInboundRoute(
                this.Routes,
                "http://testurl.com",
                "~/Admin/TestController/TestAction/3",
                new { controller = "TestController", action = "TestAction", id = "3" },
                new { area = "Admin" });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                this.Routes,
                new { area = "Admin", controller = "TestController", action = "TestAction", id = "3" });
            url.ShouldEqual("/Admin/TestController/TestAction/3");
        }

        [Test]
        public void ControllerAction()
        {
            // Inbound 
            var routeData2 = RoutingHelper.TestInboundRoute(
                this.Routes,
                "http://testurl.com",
                "~/Admin/TestController/TestAction",
                new { controller = "TestController", action = "TestAction" },
                new { area = "Admin" });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                this.Routes,
                new { area = "Admin", controller = "TestController", action = "TestAction" });
            url.ShouldEqual("/Admin/TestController/TestAction");
        }

        [Test]
        public void ListWithPaging()
        {
            // Inbound 
            var routeData2 = RoutingHelper.TestInboundRoute(
                this.Routes,
                "http://testurl.com",
                "~/Admin/AdminManager/List",
                new { controller = "AdminManager", action = "List" },
                new { area = "Admin" });

            // Outbound
            string url = RoutingHelper.GenerateOutboundUrl(
                this.Routes,
                new { area = "Admin", controller = "AdminManager", action = "List", page = "3" });
            url.ShouldEqual("/Admin/AdminManager/List/Page3");
        }
    }
}
