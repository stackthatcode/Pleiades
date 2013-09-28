using System;
using System.Web.Mvc;
using Commerce.Application.Interfaces;
using Pleiades.Application.Logging;
using Pleiades.Web;

namespace Commerce.Web.Areas.Public.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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
            var identifier = Int32.Parse(id);
            var results = _productRepository.RetrieveInfo(identifier);
            return new JsonNetResult(results);
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
