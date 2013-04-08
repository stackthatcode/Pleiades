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

        // TODO: load this into KO
        [HttpGet]
        public ActionResult ShippingMethods()
        {
            return new JsonNetResult(Context.ShippingMethods.ToList());
        }

        // TODO: load this into KO
        [HttpGet]
        public ActionResult States()
        {
            return new JsonNetResult(Context.StateTaxes.ToList());
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

            var skus = orderRequest.Items.Select(x => x.SkuCode).ToList();
            var inventory = this.Context.ProductSkus
                    .Include(x => x.Product)
                    .Where(x => skus.Contains(x.SkuCode)).ToList();

            var orderLines =
                orderRequest.Items
                    .Select(x => new OrderLine(inventory.First(y => y.SkuCode == x.SkuCode), x.Quantity))
                    .ToList();

            var stateTax =
                    this.Context.StateTaxes
                        .First(x => x.Abbreviation == orderRequest.BillingInfo.State);
            
            var shippingMethod =
                    this.Context.ShippingMethods
                        .First(x => x.Id == orderRequest.ShippingInfo.ShippingOptionId);

            var order = new Order()
            {
                EmailAddress = orderRequest.ShippingInfo.EmailAddress,
                Name = orderRequest.ShippingInfo.Name,
                Address1 = orderRequest.ShippingInfo.Address1,
                Address2 = orderRequest.ShippingInfo.Address2,
                City = orderRequest.ShippingInfo.City,
                State = orderRequest.ShippingInfo.State,
                ZipCode = orderRequest.ShippingInfo.ZipCode,
                Phone = orderRequest.ShippingInfo.Phone,

                OrderLines = orderLines,
                StateTax = stateTax,
                ShippingMethod = shippingMethod,
            };

            var paymentResponse = PaymentProcessor.AuthorizeAndCollect(orderRequest.BillingInfo, order.GrandTotal);

            // Make corrections to the Quantities - TODO
            // Return Order Creation status object...? - TODO
            // Invoke the Analytics Service - TODO

            return new JsonNetResult(paymentResponse);
        }

        public ActionResult Manager()
        {
            return View();
        }
    }
}
