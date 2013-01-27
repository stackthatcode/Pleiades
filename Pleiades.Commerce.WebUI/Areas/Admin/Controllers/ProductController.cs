using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pleiades.Data;
using Pleiades.Web;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Products;
using Commerce.Persist;

namespace Commerce.WebUI.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        IJsonCategoryRepository CategoryRepository { get; set; }
        IJsonBrandRepository BrandRepository { get; set; }
        IProductSearchRepository ProductSearchRepository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }
        PleiadesContext Context { get; set; } 

        public ProductController(
                IJsonCategoryRepository categoryRepository, IJsonBrandRepository brandRepository, 
                IProductSearchRepository productSearchRepository, PleiadesContext context)
        {
            this.CategoryRepository = categoryRepository;
            this.BrandRepository = brandRepository;
            this.ProductSearchRepository = productSearchRepository;
            this.Context = context;
        }

        [HttpGet]   
        public ActionResult Editor()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Categories()
        {
            var result = this.CategoryRepository.RetrieveAllSectionsWithCategories();
            return new JsonNetResult(result);
        }

        [HttpGet]
        public ActionResult Brands()
        {
            var result = this.BrandRepository.RetrieveAll();
            return new JsonNetResult(result);
        }
        
        [HttpGet]
        public ActionResult Search(int? brandId, int? categoryId, string searchText)
        {
            var result = this.ProductSearchRepository.FindProducts(categoryId, brandId, searchText);
            return new JsonNetResult(result);
        }

        [HttpGet]
        public ActionResult Retrieve(int id)
        {
            var result = this.ProductSearchRepository.Retrieve(id);
            return new JsonNetResult(result);
        }

        [HttpPost]
        public ActionResult Update(JsonProductSynopsis product)
        {
            Product result;
            
            result = this.Context.Products.First(x => x.Id == product.Id);
            result.Name = product.Name;
            result.Description = product.Description;
            result.Synopsis = product.Synopsis;
            result.SkuCode = product.SkuCode;
            result.SEO = product.SEO;
            result.UnitCost = product.UnitCost;
            result.UnitPrice = product.UnitPrice;
            result.Brand = this.Context.Brands.First(x => x.Id == product.BrandId);
            result.Category = this.Context.Categories.First(x => x.Id == product.CategoryId);
            this.Context.SaveChanges();
            return new JsonNetResult(result);
        }

        [HttpPost]
        public ActionResult Create()
        {
            throw new NotImplementedException();
        }
    }
}