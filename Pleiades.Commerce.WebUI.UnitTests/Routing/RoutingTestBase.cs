using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Pleiades.Commerce.WebUI;
using Pleiades.Commerce.WebUI.Areas.Admin;
using Pleiades.Framework.TestHelpers.Web;

namespace Pleiades.Commerce.WebUI.UnitTests.Routing
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
                    new AdminAreaRegistration() 
                });
            CommerceHttpApplication.RegisterDefaultArea();
        }
    }
}
