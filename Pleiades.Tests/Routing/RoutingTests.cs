using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Gallio.Framework;
using MbUnit.Framework;
using Pleiades.Storefront.WebUI;
using Pleiades.Storefront.WebUI.Areas.Admin;
using Pleiades.Storefront.WebUI.Areas.StoreFront;
using Pleiades.Tests.Utilities.Web;

namespace Pleiades.Storefront.Tests.Routing
{
    public class RoutingTests
    {
        public RouteCollection routes;

        [SetUp]
        public void TestSetup()
        {
            routes = RoutingTests.Make();
        }

        public static RouteCollection Make()
        {
            return RoutingHelper.BuildRouteList<MvcApplication>(
                new List<AreaRegistration>() 
                { 
                    new StoreFrontAreaRegistration(), 
                    new AdminAreaRegistration() 
                });
        }

    }
}
