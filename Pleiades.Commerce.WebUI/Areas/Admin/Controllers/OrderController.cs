using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Objects.SqlClient;
using System.Web;
using System.Web.Mvc;
using Pleiades.Web;
using Commerce.Persist.Concrete;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Orders;
using Commerce.Persist.Model.Billing;
using Commerce.Persist.Model.Products;

namespace Commerce.WebUI.Areas.Admin.Controllers
{
    // TODO: move this Controller to the WebAPI
    public class OrderController : Controller
    {
        PleiadesContext Context { get; set; }
        IOrderRepository OrderRepository { get; set; }

        public OrderController(PleiadesContext context, IOrderRepository orderRepository)
        {
            this.Context = context;
            this.OrderRepository = orderRepository;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // TODO: load this into KO
        [HttpGet]
        public ActionResult ShippingMethods()
        {
            return new JsonNetResult(this.Context.ShippingMethods.ToList());
        }

        // TODO: load this into KO
        [HttpGet]
        public ActionResult States()
        {
            return new JsonNetResult(this.Context.StateTaxes.ToList());
        }

        [HttpPost]
        public ActionResult SubmitOrder(OrderRequest orderRequest)
        {
            // TODO if response is false, then 500
            return new JsonNetResult(this.OrderRepository.SubmitOrder(orderRequest));
        }

        public ActionResult Manager()
        {
            return View();
        }
    }
}
