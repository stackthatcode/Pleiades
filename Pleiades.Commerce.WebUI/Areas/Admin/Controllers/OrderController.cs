using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commerce.Persist.Concrete;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Orders;

namespace Commerce.WebUI.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        PleiadesContext Context { get; set; }

        public OrderController(PleiadesContext context)
        {
            this.Context = context;
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Manager()
        {
            return View();
        }
    }
}
