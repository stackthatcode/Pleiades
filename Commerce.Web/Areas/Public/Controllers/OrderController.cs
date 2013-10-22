using System;
using System.Web.Mvc;
using Commerce.Application.Database;
using Commerce.Application.Model.Billing;
using Commerce.Application.Model.Orders;
using Pleiades.Web.Json;
using Commerce.Application.Interfaces;
using System.Linq;

namespace Commerce.Web.Areas.Public.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICartManagementService _cartManagementService;
        private readonly IOrderSubmissionService _orderSubmissionService;
        private readonly PushMarketContext _pushMarketContext;

        public OrderController(ICartManagementService cartManagementService, 
                PushMarketContext pushMarketContext, IOrderSubmissionService orderSubmissionService)
        {
            _cartManagementService = cartManagementService;
            _pushMarketContext = pushMarketContext;
            _orderSubmissionService = orderSubmissionService;
        }

        [HttpGet]
        [ActionName("action")]
        public JsonNetResult Get()
        {
            throw new NotImplementedException();
            //return new JsonNetResult(_cartManagementService.Retrieve());
        }

        [HttpPost]
        [ActionName("action")]
        public JsonNetResult Post(ShippingInfo shippingInfo, BillingInfo billingInfo)
        {
            var cart = _cartManagementService.Retrieve();
            _pushMarketContext.SaveChanges();
            var orderRequest = new OrderRequest
                {
                    Items =
                        cart.Cart.CartItems.Select(
                            x => new OrderRequestItem {Quantity = x.Quantity, SkuCode = x.Sku.SkuCode}).ToList(),
                    BillingInfo = billingInfo,
                    ShippingInfo = shippingInfo,
                };

            return JsonNetResult.Success(); 
        }
    }
}
