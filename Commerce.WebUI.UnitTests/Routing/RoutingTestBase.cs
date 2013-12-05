using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Commerce.Web;
using Commerce.Web.Plumbing;
using NUnit.Framework;
using Pleiades.TestHelpers.Web;

namespace Commerce.UnitTests.Routing
{
    public class RoutingTestBase
    {
        [TestFixtureSetUp]
        public void TestSetup()
        {
            RouteTable.Routes.Clear();

            RoutingHelper.BuildAreaRegistrations(
                new List<AreaRegistration>() 
                { 
                    new RouteRegistration(),
                    new PublicAreaRegistration(),
                });

            CommerceWebApplication.RegisterSystemRoutes();
        }
    }
}
