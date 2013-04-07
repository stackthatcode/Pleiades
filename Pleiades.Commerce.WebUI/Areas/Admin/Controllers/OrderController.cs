using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pleiades.Web;
using Commerce.Persist.Concrete;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Orders;
using Commerce.Persist.Model.Billing;
using Commerce.WebUI.Areas.Admin.Models.Order;

namespace Commerce.WebUI.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        PleiadesContext Context { get; set; }
        IPaymentProcessor PaymentProcessor { get; set; }

        public OrderController(PleiadesContext context, IPaymentProcessor paymentProcessor)
        {
            this.Context = context;
            this.PaymentProcessor = paymentProcessor;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // TODO: move this Controller to the WebAPI
        [HttpPost]
        public ActionResult SubmitOrder(OrderRequest orderRequest)
        {
            if (orderRequest.BillingInfo == null || orderRequest.ShippingInfo == null ||
                orderRequest.Items == null || orderRequest.Items.Count() == 0)
            {
                var response = new OrderResponse() { Success = false };
                response.Messages.Add("Invalid or missing data passed");
                return new JsonNetResult(response);
            }

            var order = new Order()
            {
                // Get Tax for State
                // Get Shipping Method from Shipping Option
                // Get Items from Skus - 
            };

            //var paymentResponse = PaymentProcessor.AuthorizeAndCollect(orderRequest.BillingInfo, );

            // Invoke Payment Processor - TODO
            // Get all of the Skus from the Order - TODO            
            // Make corrections to the Quantities - TODO
            // Return Order Creation status object...? - TODO
            // Invoke the Analytics Service - TODO

            return new JsonNetResult();
        }

        public ActionResult Manager()
        {
            return View();
        }
    }
}
