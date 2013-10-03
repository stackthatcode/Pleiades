using System;
using System.Linq;
using System.Web.Mvc;
using Commerce.Application.Interfaces;
using Pleiades.Application.Logging;
using Pleiades.Web;

namespace Commerce.Web.Areas.Public.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public ProductsController(IProductRepository productRepository, IInventoryRepository inventoryRepository)
        {
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
        }

        // GET api/product
        [HttpGet]
        [ActionName("action")]
        public JsonNetResult Get()
        {
            var results = _productRepository.FindProducts(null, null, "");
            return new JsonNetResult(results);
        }

        // GET api/product/5
        [HttpGet]
        [ActionName("action-with-id")]
        public JsonNetResult Get(string id)
        {
            var productid = Int32.Parse(id);
            var info = _productRepository.RetrieveInfo(productid);
            var colors = _productRepository.RetreiveColors(productid);

            if (colors.Any())
            {
                var selectedColor = colors.First();
                var inventory = _inventoryRepository.RetreiveByProductId(productid, false);
                var images = _productRepository.RetrieveImages(productid, selectedColor.Id);
                var result = new {Info = info, Inventory = inventory, Images = images};
                return new JsonNetResult(result);
            }
            else
            {
                var inventory = _inventoryRepository.RetreiveByProductId(productid, false);
                var images = _productRepository.RetrieveImages(productid);
                var result = new { Info = info, Inventory = inventory, Images = images };
                return new JsonNetResult(result);
            }
        }

        [HttpPost]
        [ActionName("operate")]
        public JsonNetResult Post(string test)
        {
            LoggerSingleton.Get().Debug("Posted:" + test);
            return new JsonNetResult(new { message = "success"});
        }
    }
}
