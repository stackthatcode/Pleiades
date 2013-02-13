using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pleiades.Data;
using Pleiades.Web;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Products;
using Commerce.Persist;
using Commerce.WebUI.Areas.Admin.Models.Color;

namespace Commerce.WebUI.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        IJsonCategoryRepository CategoryRepository { get; set; }
        IJsonBrandRepository BrandRepository { get; set; }
        IJsonSizeRepository SizeRepository { get; set; }
        IJsonColorRepository ColorRepository { get; set; }
        IProductSearchRepository ProductSearchRepository { get; set; }
        IImageBundleRepository ImageBundleRepository { get; set; }
        PleiadesContext Context { get; set; } 

        public ProductController(
                IJsonCategoryRepository categoryRepository, IJsonBrandRepository brandRepository, 
                IProductSearchRepository productSearchRepository, IJsonSizeRepository sizeRepository,
                IImageBundleRepository imageBundleRepository, PleiadesContext context)
        {
            this.CategoryRepository = categoryRepository;
            this.BrandRepository = brandRepository;
            this.SizeRepository = sizeRepository;
            this.ProductSearchRepository = productSearchRepository;
            this.ImageBundleRepository = imageBundleRepository;
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
            var result = this.ProductSearchRepository.RetrieveImages(id);
            return new JsonNetResult()
            {
                Data = result,
                Formatting = Newtonsoft.Json.Formatting.Indented,
            };
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
        public ActionResult AddProductColor(int id, int colorId)
        {
            var newColor = this.ProductSearchRepository.AddProductColor(id, colorId);
            this.Context.SaveChanges();
            return new JsonNetResult(newColor);
        }

        [HttpPost]
        public ActionResult DeleteProductColor(int id, int productColorId)
        {
            this.ProductSearchRepository.DeleteProductColor(id, productColorId);
            this.Context.SaveChanges();
            return new JsonNetResult();
        }

        [HttpPost]
        public ActionResult UpdateColorOrder(int id, string sorted)
        {
            this.ProductSearchRepository.UpdateProductColorSort(id, sorted);
            return new JsonNetResult();
        }

        [HttpPost]
        public ActionResult UpdateImageOrder(int id, string sorted)
        {
            this.ProductSearchRepository.UpdateProductImageSort(id, sorted);
            return new JsonNetResult();
        }

        [HttpPost]
        public ActionResult ChangeImageColor(int id, int productImageId, int newColorId)
        {
            this.ProductSearchRepository.ChangeImageColor(id, productImageId, newColorId);
            this.Context.SaveChanges();
            return new JsonNetResult();
        }
        
        [HttpPost]
        public ActionResult DeleteProductImage(int id, int productImageId)
        {
            this.ProductSearchRepository.DeleteProductImage(id, productImageId);
            this.Context.SaveChanges();
            return new JsonNetResult();
        }

        [HttpPost]
        public ActionResult CreateBitmap(CreateColorBitmap request)
        {
            string rgb = request.Rgb;
            var color = ColorTranslator.FromHtml(rgb);
            var imageBundle = this.ImageBundleRepository.Add(color, 150, 150);
            this.Context.SaveChanges();
            return new JsonNetResult(imageBundle);
        }

        [HttpPost]
        public ActionResult AddProductImage(int id, JsonProductImage image)
        {
            var result = this.ProductSearchRepository.AddProductImage(id, image);
            this.Context.SaveChanges();
            return new JsonNetResult(result());
        }

        [HttpPost]
        public ActionResult AssignImagesToColor(int id)
        {
            this.ProductSearchRepository.AssignImagesToColor(id);
            this.Context.SaveChanges();
            return new JsonNetResult();
        }

        [HttpPost]
        public ActionResult UnassignImagesFromColor(int id)
        {
            this.ProductSearchRepository.UnassignImagesFromColor(id);
            this.Context.SaveChanges();
            return new JsonNetResult();
        }
    }
}