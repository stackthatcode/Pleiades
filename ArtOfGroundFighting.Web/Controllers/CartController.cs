using System.Web.Mvc;
using ArtOfGroundFighting.Web.Models;
using Commerce.Application.Database;
using Commerce.Application.Products;
using Commerce.Application.Shopping;
using Pleiades.Web.Json;

namespace ArtOfGroundFighting.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartManagementService _cartManagementService;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly PushMarketContext _pushMarketContext;

        public CartController(
                ICartManagementService cartManagementService, 
                IInventoryRepository inventoryRepository,
                PushMarketContext pushMarketContext)
        {
            _cartManagementService = cartManagementService;
            _inventoryRepository = inventoryRepository;
            _pushMarketContext = pushMarketContext;
        }

        [HttpGet]
        [ActionName("action")]
        public JsonNetResult Get()
        {
            var result = _cartManagementService.Retrieve();
            _pushMarketContext.SaveChanges();
            return new JsonNetResult(result);
        }

        [HttpPost]
        [ActionName("action")]
        public JsonNetResult Post(string skuCode, int quantity)
        {
            var result =_cartManagementService.AddQuantity(skuCode, quantity);
            var cart = _cartManagementService.Retrieve();
            var sku = _inventoryRepository.RetreiveBySkuCode(skuCode);
            var inventory = _inventoryRepository.RetreiveByProductId(sku.Product.Id, false);
            _pushMarketContext.SaveChanges();

            return JsonExtensions.BuildCartChangeResponseJson((int)result, cart, inventory);
        }

        [HttpPut]
        [ActionName("action")]
        public JsonNetResult Put(string skuCode, int? quantity, int? shippingMethodId, string stateTaxAbbr)
        {
            if (skuCode != null && quantity != null)
            {
                var result = _cartManagementService.UpdateQuantity(skuCode, quantity.Value);
                _pushMarketContext.SaveChanges();
                return new JsonNetResult(result);
            }
            if (shippingMethodId != null)
            {
                _cartManagementService.UpdateShippingMethod(shippingMethodId.Value);
                _pushMarketContext.SaveChanges();
                return new JsonNetResult(_cartManagementService.Retrieve());
            }
            if (stateTaxAbbr != null)
            {
                _cartManagementService.UpdateStateTax(stateTaxAbbr);
                _pushMarketContext.SaveChanges();
                return new JsonNetResult(_cartManagementService.Retrieve());                
            }
            return new JsonNetResult(_cartManagementService.Retrieve());
        }

        [HttpDelete]
        [ActionName("action")]
        public JsonNetResult Delete(string skuCode)
        {
            var result = _cartManagementService.RemoveItem(skuCode);
            _pushMarketContext.SaveChanges();
            return new JsonNetResult(result);
        }
    }
}
