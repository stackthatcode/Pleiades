using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Commerce.WebUI;
using Pleiades.Framework.TestHelpers;
using Pleiades.Framework.TestHelpers.Web;

namespace Pleiades.Commerce.WebUI.UnitTests.Routing
{
    [TestFixture]
    public class DefaultRouteTests : RoutingTestBase
    {
        [Test]
        public void VerifyThatSlashReturnsTheDefaultRoute()
        {
            // NOTE: there are no Data Tokens on the default route, because it's not in the Area -- it just
            // directs traffic stuff to the Products Controller

            // Inbound-only
            var routeData2 =
                RoutingHelper.VerifyInboundRoute(
                    RouteTable.Routes,
                    inboundUrl: "~/",
                    expectedRouteValues: 
                        new { area = "Public", controller = "Products", action = "List", category = (string)null },
                    expectedDataTokens: new { });
        }
    }
}
