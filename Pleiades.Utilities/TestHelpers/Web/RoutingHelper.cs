using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;
using MbUnit.Framework;
using Rhino.Mocks;
using Pleiades.Utilities.TestHelpers.General;
using Pleiades.Utilities.General;


namespace Pleiades.Utilities.TestHelpers.Web
{
    public class RoutingHelper
    {
        /// <summary>
        /// Crucial: initializes the Routes
        /// </summary>
        public static RouteCollection BuildRouteList<TAppType>(List<AreaRegistration> areaRegistrations)
                    where TAppType : System.Web.HttpApplication
        {
            // Create a Route Collection for the Application
            var routes = new RouteCollection();

            // IMPORTANT - pay attention to which set of Routes are loaded first, as that will affect
            // .. the ordering in the Routing Table
            
            // Load the area Registration routes first
            foreach (var registration in areaRegistrations)
            {
                var areaRegistrationContext = new AreaRegistrationContext(registration.AreaName, routes);
                registration.RegisterArea(areaRegistrationContext);
            }

            //  Invoke the Register Routes method
            Type appType = typeof(TAppType);
            MethodInfo registerMethod = appType.GetMethod("RegisterRoutes");
            Object myStaticMethodResult = registerMethod.Invoke(null, new object[] { routes });

            return routes;
        }

        /// <param name="routes">The Routing Dictionary</param>
        /// <param name="values">Anonymous object with Route Dictionary</param>
        /// <returns></returns>
        public static string GenerateOutboundUrl(RouteCollection routes, object values)
        {
            var currentRouteData = new RouteData();
            var httpcontext = HttpContextStubFactory.Make(AppRelativeCurrentExecutionFilePath: null);

            RequestContext requestcontext = new RequestContext(httpcontext, currentRouteData);

            return UrlHelper.GenerateUrl(null, null, null,
                    new RouteValueDictionary(values), routes, requestcontext, true);
        }

        /// <summary>
        /// Convenience method for Testing Routes: tests the Route extracted from the AppRelativePath
        /// <returns></returns>
        public static RouteData TestInboundRoute(RouteCollection routes, 
                string Url, string Path, object expectedValues, object expectedDataTokens)
        {
            // This passes
            var httpcontext = HttpContextStubFactory.Make(Url: Url, AppRelativeCurrentExecutionFilePath: Path);

            // Attempt to match the Route
            var routeData = routes.GetRouteData(httpcontext);

            // Verification for non-matches
            if (expectedValues == null)
            {
                Assert.IsNull(routeData);
                return null;
            }

            // Verification for explicit match
            var expectedRouteDictionary = new RouteValueDictionary(expectedValues);
            foreach (var expectedValue in expectedRouteDictionary)
            {
                if (expectedValue.Value == null)
                    Assert.IsNull(routeData.Values[expectedValue.Key]);
                else
                    routeData.Values[expectedValue.Key].ShouldEqual(expectedValue.Value);
            }

            var expectedDataTokenDictionary = new RouteValueDictionary(expectedDataTokens);
            foreach (var expectedToken in expectedDataTokenDictionary)
            {
                routeData.DataTokens[expectedToken.Key].ShouldEqual(expectedToken.Value);
            }

            return routeData;
        }
    }
}
