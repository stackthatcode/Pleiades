using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Commerce.WebUI;
using Commerce.WebUI.Areas.Admin;
using Commerce.WebUI.Areas.Public;
using Pleiades.TestHelpers.Web;

namespace Commerce.WebUI.UnitTests.Routing
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
                   // new AdminAreaRegistration(),
                    new PublicAreaRegistration(),
                });

            CommerceHttpApplication.RegisterRoutes();
        }
    }
}
