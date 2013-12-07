using System.Web.Routing;
using Commerce.Web.Plumbing;
using NUnit.Framework;

namespace Commerce.UnitTests.Routing
{
    public class RoutingTestBase
    {
        [TestFixtureSetUp]
        public void TestSetup()
        {
            RouteTable.Routes.Clear();
            RouteRegistration.RegisterRoutes(RouteTable.Routes);
        }
    }
}
