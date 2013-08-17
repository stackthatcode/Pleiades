using System;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Pleiades.Helpers;


namespace Pleiades.TestHelpers.Web
{
    public static class ControllerHelpers
    {
        public static void ShouldBeRedirectionTo(this ActionResult actionResult, string expectedURL)
        {
            ((RedirectResult)actionResult).Url.ShouldEqual(expectedURL);
        }

        public static void ShouldBeRedirectionTo(this ActionResult actionResult, object expectedRouteAnonymousObject)
        {
            var actualRouteDict = ((RedirectToRouteResult)actionResult).RouteValues;
            var expectedRouteDict = new RouteValueDictionary(expectedRouteAnonymousObject);
            expectedRouteDict.ForEach(item => actualRouteDict[item.Key].ShouldEqual(item.Value));
        }

        public static void ShouldBeRedirectionTo(this ActionResult actionResult, RouteValueDictionary expectedRouteDictionary)
        {
            var actualRouteDict = ((RedirectToRouteResult)actionResult).RouteValues;
            expectedRouteDictionary.ForEach(item => actualRouteDict[item.Key].ShouldEqual(item.Value));
        }

        public static void ShouldBeDefaultView(this ActionResult actionResult)
        {
            actionResult.ShouldBeView(String.Empty);
        }

        public static void ShouldBeView(this ActionResult actionResult)
        {
            Assert.IsTrue(actionResult is ViewResult);
        }

        public static void ShouldBeView(this ActionResult actionResult, string viewName)
        {
            Assert.IsTrue(actionResult is ViewResult);

            // Didn't know there was a View Name property.  Hmph!
            ((ViewResult)actionResult).ViewName.ShouldEqual(viewName);
        }

        public static T WithIncomingValues<T>(this T controller, FormCollection values) where T : Controller
        {
            controller.ControllerContext = new ControllerContext();
            controller.ValueProvider = 
                new NameValueCollectionValueProvider(
                    values, System.Globalization.CultureInfo.CurrentCulture);

            return controller;
        }

        /*
        public static ViewContext DynamicViewContext(this MockRepository mocks, HttpContextParams context, string viewName)
        {
            var httpContext = mocks.SetupResultsForHttpContext(context);
            var controller = mocks.Stub<ControllerBase>();
            var view = mocks.Stub<IView>();
            mocks.Replay(controller);

            var controllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            return new ViewContext(controllerContext, view, new ViewDataDictionary(), new TempDataDictionary(), new StringWriter());
        }

        public static TController SetupControllerContext<TController>(this TController controller) where TController : Controller
        {
            var controllerContext = new ControllerContext(Make(), new RouteData(), controller);
            controller.ControllerContext = controllerContext;
            return controller;
        }
         */
    }
}
