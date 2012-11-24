using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using NUnit.Framework;
using Rhino.Mocks;
using Pleiades.TestHelpers;
using Pleiades.TestHelpers.Web;

namespace Pleiades.Web.Tests.HttpContext
{
    [TestFixture]
    public class HttpContextMockTests
    {
        [Test]
        public void PlayingWithMocksVerifyingStatusCodeSet()
        {
            // Mock the HttpContext object
            var context = HttpContextStubFactory.Create();            

            // The object which relies on this mock should set status code...
            context.Response.StatusCode = 500;

            // Verify
            context.Response.StatusCode.ShouldEqual(500);
        }

        [Test]
        public void PlayingWithMocksVerifyingSessionVariableSet()
        {
            // Mock the HttpContext object
            var context = HttpContextStubFactory.Create();

            // Set the Session Variable
            context.Session["testa"] = "anna";

            // Verify
            context.Session["testa"].ShouldEqual("anna");
        }
    }
}