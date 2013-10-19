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
        [ActionName("action")]
        public JsonNetResult Post(string skuCode, int quantity)
        {
            var result =_cartManagementService.AddQuantity(skuCode, quantity);
            _pushMarketContext.SaveChanges();
            return new JsonNetResult(new { CartResponseCode = (int)result });
        }

        [HttpPut]
        [ActionName("action")]
        public JsonNetResult Put(string skuCode, int quantity)
        {
            var result = _cartManagementService.UpdateQuantity(skuCode, quantity);
            _pushMarketContext.SaveChanges();
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
