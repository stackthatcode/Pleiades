﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Gallio.Framework;
using MbUnit.Framework;
using Pleiades.Commerce.WebUI;
using Pleiades.Commerce.WebUI.Areas.Admin;
using Pleiades.Utilities.TestHelpers.Web;

namespace Pleiades.Commerce.UnitTests.WebUI.Routing
{
    public class RoutingTestBase
    {
        public RouteCollection Routes { get; set; }

        [SetUp]
        public void TestSetup()
        {
            Routes = RoutingHelper.BuildRouteList<MvcApplication>(
                new List<AreaRegistration>() 
                { 
                    new AdminAreaRegistration() 
                });
        }
    }
}
