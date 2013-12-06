using System;
using System.Web.Mvc;
using Commerce.Application.Lists;
using Commerce.Application.Lists.Entities;
using Commerce.Application.Products;
using Commerce.ArtOfGroundFighting.Models;
using Pleiades.App.Logging;
using Pleiades.Web.Json;

namespace Commerce.ArtOfGroundFighting.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IJsonBrandRepository _jsonBrandRepository;

        public ProductsController(IProductRepository productRepository, 
                IInventoryRepository inventoryRepository, IJsonBrandRepository jsonBrandRepository)
        {
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _jsonBrandRepository = jsonBrandRepository;
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
            var sizes = _productRepository.RetrieveSizes(productid);

            JsonBrand brand = null;
            if (info.BrandId.HasValue)
            {
                brand = _jsonBrandRepository.Retrieve(info.BrandId.Value);
            }

            var images = _productRepository.RetrieveImages(productid);
            var inventory = _inventoryRepository.RetreiveByProductId(productid, false);

            return JsonExtensions.BuildProductDetailJson(info, brand, colors, sizes, images, inventory);
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
