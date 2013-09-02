using System;
using System.Web.Mvc;
using Commerce.Web.Areas.Public.Models;
using Pleiades.Application.Logging;

namespace Commerce.Web.Areas.Public.Controllers
{
    public class ProductsController : Controller
    {
        public ActionResult List()
        {
            LoggerSingleton.Get().Debug("Test Logger");

            try
            {
                throw new Exception("Oh noes!!!");
            }
            catch (Exception ex)
            {
                LoggerSingleton.Get().Error(ex);
            }

            var model = new ProductListModel 
                { Name = "arthur", AccountBalance = 999, SelectedValueForRadio = "1", SelectedValueForDropDownList = "2" };

            return View(model);
        }

        [HttpPost]
        public ActionResult List(ProductListModel model)
        {
            return View(model);
        }
    }
}
