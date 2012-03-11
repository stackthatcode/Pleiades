using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using MbUnit.Framework;
using Pleiades.Utilities.General;
using Pleiades.Utilities.TestHelpers.General;

namespace Pleiades.Utilities.TestHelpers.Web
{
    public static class ControllerHelpers
    {
        public static void ShouldBeRedirectionTo(this ActionResult actionResult, object expectedRouteValues)
        {
            var actualRouteDict = ((RedirectToRouteResult)actionResult).RouteValues;
            var expectedRouteDict = new RouteValueDictionary(expectedRouteValues);
            expectedRouteDict.ForEach(item => actualRouteDict[item.Key].ShouldEqual(item.Value));
        }

        // Not to be confused - ViewName == "" is not the same as Index
        public static void ShouldBeDefaultView(this ActionResult actionResult)
        {
            actionResult.ShouldBeView(String.Empty);
        }

        public static void ShouldBeView(this ActionResult actionResult)
        {
            Assert.IsInstanceOfType<ViewResult>(actionResult);
        }

        public static void ShouldBeView(this ActionResult actionResult, string viewName)
        {
            Assert.IsInstanceOfType<ViewResult>(actionResult);

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
