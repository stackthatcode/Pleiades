﻿using System;
using NUnit.Framework;
using Pleiades.TestHelpers;
using Pleiades.TestHelpers.Web;
using Commerce.WebUI.Areas.Admin.Controllers;

namespace Commerce.WebUI.UnitTests.Controllers.Admin
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public void Index_Should_Return_Default_View()
        {
            var controller = new HomeController();
            var result = controller.Index();
            result.ShouldBeDefaultView();
        }
    }
}