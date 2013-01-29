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
        IJsonSizeRepository SizeRepository { get; set; }
        IJsonColorRepository ColorRepository { get; set; }
        IProductSearchRepository ProductSearchRepository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }
        PleiadesContext Context { get; set; } 

        public ProductController(
                IJsonCategoryRepository categoryRepository, IJsonBrandRepository brandRepository, 
                IProductSearchRepository productSearchRepository, IJsonSizeRepository sizeRepository,
                PleiadesContext context)
        {
            this.CategoryRepository = categoryRepository;
            this.BrandRepository = brandRepository;
            this.SizeRepository = sizeRepository;
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
        public ActionResult SizeGroups()
        {
            var result = this.SizeRepository.RetrieveAll(true);
            return new JsonNetResult(result);
        }
        
        [HttpGet]
        public ActionResult Search(int? brandId, int? categoryId, string searchText)
        {
            var result = this.ProductSearchRepository.FindProducts(categoryId, brandId, searchText);
            return new JsonNetResult(result);
        }

        [HttpGet]
        public ActionResult Info(int id)
        {
            var result = this.ProductSearchRepository.RetrieveInfo(id);
            return new JsonNetResult(result);
        }

        [HttpPost]
        public ActionResult Insert(JsonProductInfo product)
        {
            return Save(product);
        }

        [HttpPost]
        public ActionResult Update(JsonProductInfo product)
        {
            return Save(product);
        }

        [HttpGet]
        public ActionResult Colors(int id)
        {
            var result = this.ProductSearchRepository.RetreieveColors(id);
            return new JsonNetResult(result);
        }


        [HttpGet]
        public ActionResult Images(int id)
        {
            var result = this.ProductSearchRepository.RetrieveInfo(id);
            return new JsonNetResult(result);
        }

        private ActionResult Save(JsonProductInfo product)
        {
            Product result;
            if (product.Id == null) 
            {
                result = new Product();
                this.Context.Products.Add(result);
            }
            else
            {
                result = this.Context.Products.First(x => x.Id == product.Id);
            }
            result.Name = product.Name;
            result.Description = product.Description;
            result.Synopsis = product.Synopsis;
            result.SkuCode = product.SkuCode;
            result.SEO = product.SEO;
            result.UnitCost = product.UnitCost;
            result.UnitPrice = product.UnitPrice;
            result.Brand = this.Context.Brands.First(x => x.Id == product.BrandId);
            result.Category = this.Context.Categories.First(x => x.Id == product.CategoryId);
            result.SizeGroup = this.Context.SizeGroups.FirstOrDefault(x => x.ID == product.SizeGroupId);
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