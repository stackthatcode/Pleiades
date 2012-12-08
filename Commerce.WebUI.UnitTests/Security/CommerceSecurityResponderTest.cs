using System;
using System.Net;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.Web.Security.Model;
using Pleiades.TestHelpers.Web;
using Commerce.WebUI.Plumbing;

namespace Commerce.WebUI.UnitTests.Security
{
    [TestFixture]
    public class CommerceSecurityResponderTest
    {
        [Test]
        public void Verify_AccessDeniedSolicitLogon()
        {
            // Arrange
            var context = MockRepository.GenerateStub<AuthorizationContext>();
            context.HttpContext = HttpContextStubFactory.Create(Url: "http://go.com");

            // Act
            var responder = new SecurityResponder();
            responder.Process(SecurityCode.AccessDeniedSolicitLogon, context);

            var expectedRouteDict = OutboundNavigation.AdminLogin();
            // expectedRouteDict["returlUrl"] = "http://go.com";  TODO => Figure out how to mock Server.UrlEncode

            // Assesrt
            Assert.AreEqual(context.HttpContext.Response.StatusCode, 302);
            context.Result.ShouldBeRedirectionTo(expectedRouteDict);
        }
    }
}