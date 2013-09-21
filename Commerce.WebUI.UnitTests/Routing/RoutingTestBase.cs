using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Commerce.Web.Areas.Public;
using NUnit.Framework;
using Commerce.Web.Areas.Admin;
using Pleiades.TestHelpers.Web;

namespace Commerce.Web.UnitTests.Routing
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
                    new AdminAreaRegistration(),
                    new PublicAreaRegistration(),
                });

            CommerceWebApplication.RegisterSystemRoutes();
        }
    }
}
