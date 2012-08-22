using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Routing;
using System.Web.Mvc;
using Rhino.Mocks;
using NUnit.Framework;

namespace Pleiades.Framework.TestHelpers.Web
{
    public static class RoutingHelper
    {
        public static void BuildAreaRegistrations(List<AreaRegistration> areaRegistrations)
        {
            // Load the area Registration routes first
            foreach (var registration in areaRegistrations)
            {
                var areaRegistrationContext = new AreaRegistrationContext(registration.AreaName, RouteTable.Routes);
                registration.RegisterArea(areaRegistrationContext);
            }
        }

        /// <summary>
        /// Given an established collection of Routes, will apply values to create an Outbound Url
        /// </summary>
        public static string GenerateOutboundUrl(RouteCollection routes, object outboundValues)
        {
            var httpcontext = HttpContextStubFactory.Make(AppRelativeCurrentExecutionFilePath: null);
            var requestcontext = new RequestContext(httpcontext, new RouteData());

            return UrlHelper.GenerateUrl(null, null, null,
                    new RouteValueDictionary(outboundValues), routes, requestcontext, true);
        }

        /// <summary>
        /// Convenience method for Testing Routes: tests the Route extracted from the AppRelativePath
        /// </summary>
        public static RouteData VerifyInboundRoute(
                RouteCollection routes, string currentApplicationPath = "http://testurl.com", string inboundUrl = "/",
                object expectedRouteValues = null, object expectedDataTokens = null)
        {
            // This passes
            var httpcontext = HttpContextStubFactory.Make(
                Url: currentApplicationPath, AppRelativeCurrentExecutionFilePath: inboundUrl);

            // Attempt to match the Route
            var routeData = routes.GetRouteData(httpcontext);

            // Verification for non-matches
            if (expectedRouteValues == null)
            {
                Assert.IsNull(routeData);
                return null;
            }

            // Verification for explicit match
            var expectedRouteDictionary = new RouteValueDictionary(expectedRouteValues);
            var expectedDataTokenDictionary = new RouteValueDictionary(expectedDataTokens);

            foreach (var expectedValue in expectedRouteDictionary)
            {
                if (expectedValue.Value == null)
                    Assert.IsNull(routeData.Values[expectedValue.Key]);
                else
                    routeData.Values[expectedValue.Key].ShouldEqual(expectedValue.Value);
            }
            foreach (var expectedToken in expectedDataTokenDictionary)
            {
                routeData.DataTokens[expectedToken.Key].ShouldEqual(expectedToken.Value);
            }

            return routeData;
        }

        public static void MatchesExpectedRouteDictionary(this RouteValueDictionary actual, RouteValueDictionary expected)
        {
            actual.ToList().ForEach(actualKVP => Assert.AreEqual(expected[actualKVP.Key], actualKVP.Value));
            expected.ToList().ForEach(expectedKVP => Assert.AreEqual(expected[expectedKVP.Key], expectedKVP.Value));
        }
    }
}
