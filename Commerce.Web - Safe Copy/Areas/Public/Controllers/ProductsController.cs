using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commerce.Web.Areas.Public.Models;

namespace Commerce.Web.Areas.Public.Controllers
{
    public class ProductsController : Controller
    {
        //
        // GET: /Products/

        public ActionResult List()
        {
            var model = new ProductListModel 
                { Name = "arthur", AccountBalance = 999, SelectedValueForRadio = "1", SelectedValueForDropDownList = "2" };

            return View(model);
        }

        [HttpPost]
        public ActionResult List(ProductListModel model)
        {
            return View(model);
        }

        //
        // GET: /Products/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

    }
}
