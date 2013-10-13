using System.Web.Mvc;
using Commerce.Application.Database;
using Pleiades.Web.Json;
using Commerce.Application.Interfaces;

namespace Commerce.Web.Areas.Public.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartManagementService _cartManagementService;
        private readonly PushMarketContext _pushMarketContext;

        public CartController(ICartManagementService cartManagementService, PushMarketContext pushMarketContext)
        {
            _cartManagementService = cartManagementService;
            _pushMarketContext = pushMarketContext;
        }

        [HttpGet]
        [ActionName("action")]
        public JsonNetResult Get()
        {
            return new JsonNetResult(_cartManagementService.Retrieve());
        }

        [HttpPost]
        [ActionName("operate")]
        public JsonNetResult Post(string skuCode, int quantity)
        {
            var result =_cartManagementService.AddQuantity(skuCode, quantity);
            _pushMarketContext.SaveChanges();
            return new JsonNetResult(new { ActualQuantity = result });
        }

        [HttpPut]
        [ActionName("operate")]
        public JsonNetResult Put(string skuCode, int quantity)
        {
            var result = _cartManagementService.UpdateQuantity(skuCode, quantity);
            _pushMarketContext.SaveChanges();
            return new JsonNetResult(new { ActualQuantity = result });
        }

        [HttpDelete]
        [ActionName("operate")]
        public JsonNetResult Delete(string skuCode)
        {
            _cartManagementService.RemoveItem(skuCode);
            return JsonNetResult.Success();
        }

    }
}
