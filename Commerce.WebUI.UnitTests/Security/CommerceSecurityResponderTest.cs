using System.Web.Mvc;
using Commerce.Web;
using NUnit.Framework;
using Pleiades.TestHelpers.Web;
using Rhino.Mocks;
using Pleiades.Web.Security.Model;
using Commerce.Web.Plumbing;

namespace Commerce.UnitTests.Security
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

            var expectedRouteDict = Navigation.Login();
            // expectedRouteDict["returlUrl"] = "http://go.com";  TODO => Figure out how to mock Server.UrlEncode

            // Assesrt
            Assert.AreEqual(context.HttpContext.Response.StatusCode, 302);
            context.Result.ShouldBeRedirectionTo(expectedRouteDict);
        }
    }
}