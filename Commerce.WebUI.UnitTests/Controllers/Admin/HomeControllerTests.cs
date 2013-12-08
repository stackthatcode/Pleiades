using Commerce.Web.Controllers;
using NUnit.Framework;
using Pleiades.TestHelpers.Web;

namespace Commerce.UnitTests.Controllers.Admin
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