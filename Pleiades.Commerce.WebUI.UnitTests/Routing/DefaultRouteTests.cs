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
        // Wierd but True stuff about Routes

        // NOTE #1: there are no Data Tokens on the default route, because it's not in the Area 
        // ... it just directs traffic stuff to the Products Controller.  Same concept as Note #2.

        // UPDATE: MVC was unable to find the Views

        // NOTE #2: when generating an Outbound URL, we don't use Data Tokens, since we don't know which Route is
        // ... going to match our Route Data.

        // NOTE #3: when parsing an Inbound URL, a Data Token will be in the Route Data of the matching Route

        [Test]
        public void VerifyThatSlashReturnsTheDefaultRoute()
        {
            // Inbound-only
            var routeData2 =
                RoutingHelper.VerifyInboundRoute(
                    RouteTable.Routes,
                    inboundUrl: "~/",
                    expectedRouteValues: 
                        new { controller = "Products", action = "List", category = (string)null },
                    expectedDataTokens: new { area = "Public" });
        }
    }
}
