using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Rhino.Mocks;

namespace Pleiades.Utilities.TestHelpers.Web
{
    /// <summary>
    /// Pipe dream, mayhaps?
    /// </summary>
    public class HtmlHelperMockFactory
    {
        public static HtmlHelper<T> Make<T>()
        {
            var mockViewContext = MockRepository.GenerateStub<ViewContext>();
            mockViewContext.HttpContext = HttpContextStubFactory.Make();
            mockViewContext.RouteData = new RouteData();
            mockViewContext.ViewData = new ViewDataDictionary();
            mockViewContext.Controller = MockRepository.GenerateStub<ControllerBase>();
            mockViewContext.View = MockRepository.GenerateStub<IView>();
            mockViewContext.TempData = new TempDataDictionary();

            var mockViewDataContainer = MockRepository.GenerateStub<IViewDataContainer>();
            mockViewDataContainer.ViewData = new ViewDataDictionary();

            return new HtmlHelper<T>(mockViewContext, mockViewDataContainer, new RouteCollection());
        }
    }
}
