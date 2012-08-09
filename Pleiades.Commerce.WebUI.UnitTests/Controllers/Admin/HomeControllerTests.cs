using System;
using NUnit.Framework;
using Pleiades.Framework.TestHelpers;
using Pleiades.Framework.TestHelpers.Web;
using Pleiades.Commerce.WebUI.Areas.Admin.Controllers;

namespace Pleiades.Commerce.WebUI.UnitTests.Controllers.Admin
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