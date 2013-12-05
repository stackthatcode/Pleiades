using System;
using System.Web.Mvc;
using Commerce.Application.Database;
using Commerce.Application.Orders;
using Commerce.Application.Orders.Entities;
using Commerce.Application.Shopping;
using Pleiades.Web.Json;
using System.Linq;

namespace Commerce.Web.Areas.Public.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICartManagementService _cartManagementService;
        private readonly ICartIdentificationService _cartIdentificationService;
        private readonly IOrderService _orderSubmissionService;
        private readonly PushMarketContext _pushMarketContext;

        public OrderController(
                ICartManagementService cartManagementService, 
                ICartIdentificationService cartIdentificationService,
                PushMarketContext pushMarketContext, 
                IOrderService orderSubmissionService)
        {
            _cartManagementService = cartManagementService;
            _cartIdentificationService = cartIdentificationService;
            _pushMarketContext = pushMarketContext;
            _orderSubmissionService = orderSubmissionService;
        }

        [HttpGet]
        [ActionName("DISABLED")]
        public JsonNetResult Get()
        {
            var cart = _cartManagementService.Retrieve();
            var order = _orderSubmissionService.Retreive(cart.Cart.OrderExternalId);
            return new JsonNetResult(order);
        }

        [HttpPost]
        [ActionName("action")]
        public JsonNetResult Post(ShippingInfo shippingInfo, string token)
        {
            var cart = _cartManagementService.Retrieve();
            var orderRequest = new OrderRequest
                {
                    Items =
                        cart.Cart.CartItems.Select(
                            x => new OrderRequestItem {Quantity = x.Quantity, SkuCode = x.Sku.SkuCode}).ToList(),
                    Token = token,
                    ShippingInfo = shippingInfo,
                };

            var result = _orderSubmissionService.Submit(orderRequest);
            if (result.Success == true)
            {
                _cartIdentificationService.ProvisionNewCartId();
                cart.Cart.OrderExternalId = result.Order.ExternalId;
            }

            _pushMarketContext.SaveChanges();
            return new JsonNetResult(result); 
        }
    }
}
