using System;
using System.Net;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Framework.Security;
using Pleiades.Framework.TestHelpers.Web;
using Pleiades.Commerce.WebUI.Plumbing.Navigation;
using Pleiades.Commerce.WebUI.Plumbing.Security;

namespace Pleiades.Commerce.WebUI.UnitTests.Security
{
    [TestFixture]
    public class TestForMySecurityCodeResponder
    {
        [Test]
        public void Verify_AccessDeniedSolicitLogon()
        {
            // Arrange
            var context = MockRepository.GenerateStub<AuthorizationContext>();
            context.HttpContext = HttpContextStubFactory.Make(Url: "http://go.com");

            // Act
            var responder = new MySecurityCodeResponder();
            responder.Execute(SecurityResponseCode.AccessDeniedSolicitLogon, context);

            var expectedRouteDict = OutboundNavigation.AdminLogin();
            // expectedRouteDict["returlUrl"] = "http://go.com";  TODO => Figure out how to mock Server.UrlEncode

            // Assesrt
            Assert.AreEqual(context.HttpContext.Response.StatusCode, 302);
            context.Result.ShouldBeRedirectionTo(expectedRouteDict);
        }
    }
}