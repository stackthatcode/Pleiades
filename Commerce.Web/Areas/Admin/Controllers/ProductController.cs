using System;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using Commerce.Application.Database;
using Pleiades.Web;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Products;
using Commerce.Web.Areas.Admin.Models.Color;

namespace Commerce.Web.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        IJsonCategoryRepository CategoryRepository { get; set; }
        IJsonBrandRepository BrandRepository { get; set; }
        IJsonSizeRepository SizeRepository { get; set; }
        IJsonColorRepository ColorRepository { get; set; }
        IProductRepository ProductRepository { get; set; }
        IInventoryRepository InventoryRepository { get; set; }
        IImageBundleRepository ImageBundleRepository { get; set; }
        PushMarketContext Context { get; set; } 

        public ProductController(IJsonCategoryRepository categoryRepository, 
                IJsonBrandRepository brandRepository, IProductRepository productRepository, 
                IJsonSizeRepository sizeRepository, IInventoryRepository inventoryRepository,
                IImageBundleRepository imageBundleRepository, PushMarketContext context)
        {
            this.CategoryRepository = categoryRepository;
            this.BrandRepository = brandRepository;
            this.SizeRepository = sizeRepository;
            this.ProductRepository = productRepository;
            this.InventoryRepository = inventoryRepository;
            this.ImageBundleRepository = imageBundleRepository;
            this.Context = context;
        }

        [HttpGet]   
        public ActionResult Editor()
        {
            return View();
        }

        // todo: move this over to Category Controller
        [HttpGet]
        public ActionResult Categories()
        {
            throw new Exception("Ajax Bomb!");
            var result = this.CategoryRepository.RetrieveAllSectionsWithCategories();
            return new JsonNetResult(result);
        }

        // todo: move this over to Brand Controller
        [HttpGet]
        public ActionResult Brands()
        {
            var result = this.BrandRepository.RetrieveAll();
            return new JsonNetResult(result);
        }

        // todo: move this over to Size Controller
        [HttpGet]
        public ActionResult SizeGroups()
        {
            var result = this.SizeRepository.RetrieveAll(true);
            return new JsonNetResult(result);
        }
        


        // Info
        [HttpGet]
        public ActionResult Search(int? brandId, int? categoryId, string searchText)
        {
            var result = this.ProductRepository.FindProducts(categoryId, brandId, searchText);
            return new JsonNetResult(result);
        }

        [HttpGet]
        public ActionResult Info(int id)
        {
            var result = this.ProductRepository.RetrieveInfo(id);
            return new JsonNetResult(result);
        }

        [HttpPost]
        public ActionResult Save(JsonProductInfo product)
        {
            Product result;
            if (product.Id == null)
            {
                result = new Product();
                this.Context.Products.Add(result);
            }
            else
            {
                result = 
                    this.Context.Products
                        .Include(x => x.Brand)
                        .Include(x => x.Category)
                        .First(x => x.Id == product.Id);
                    
                if (product.SkuCode != result.SkuCode) 
                {
                    // Update the Inventory Sku Code
                    this.InventoryRepository.UpdateSkuCode(product.Id.Value, product.SkuCode);
                }
            }

            result.Name = product.Name;
            result.Description = product.Description;
            result.Synopsis = product.Synopsis;
            result.SkuCode = product.SkuCode;
            result.SEO = product.SEO;
            result.UnitCost = product.UnitCost;
            result.UnitPrice = product.UnitPrice;
            result.Brand = this.Context.Brands.FirstOrDefault(x => x.Id == product.BrandId);
            result.Category = this.Context.Categories.FirstOrDefault(x => x.Id == product.CategoryId);

            this.Context.SaveChanges();

            return new JsonNetResult(result.ToJson());
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
             this.ProductRepository.Delete(id);
            this.Context.SaveChanges();
            return JsonNetResult.Success();
        }



        // Colors
        [HttpGet]
        public ActionResult Colors(int id)
        {
            var result = this.ProductRepository.RetreiveColors(id);
            return new JsonNetResult(result);
        }

        [HttpPost]
        public ActionResult AddProductColor(int id, int colorId)
        {
            var newColor = this.ProductRepository.AddProductColor(id, colorId);
            this.Context.SaveChanges();
            return new JsonNetResult(newColor);
        }

        [HttpPost]
        public ActionResult DeleteProductColor(int id, int productColorId)
        {
            this.ProductRepository.DeleteProductColor(id, productColorId);
            this.Context.SaveChanges();
            return JsonNetResult.Success();
        }

        [HttpPost]
        public ActionResult UpdateColorOrder(int id, string sorted)
        {
            this.ProductRepository.UpdateProductColorSort(id, sorted);
            return JsonNetResult.Success();
        }



        // Sizes
        [HttpGet]
        public ActionResult Sizes(int id)
        {
            var result = this.ProductRepository.RetrieveSizes(id);
            return new JsonNetResult(result);
        }

        [HttpPost]
        public ActionResult AddProductSize(int id, int sizeId)
        {
            var size = this.ProductRepository.AddProductSize(id, sizeId);
            this.Context.SaveChanges();
            return new JsonNetResult(size);
        }

        [HttpPost]
        public ActionResult CreateProductSize(int id, ProductSize size)
        {
            var result = this.ProductRepository.CreateProductSize(id, size);
            this.Context.SaveChanges();
            return new JsonNetResult(result());
        }

        [HttpPost]
        public ActionResult UpdateSizeOrder(int id, string sorted)
        {
            this.ProductRepository.UpdateSizeOrder(id, sorted);
            return JsonNetResult.Success();
        }

        [HttpPost]
        public ActionResult DeleteProductSize(int id, int sizeId)
        {
            this.ProductRepository.DeleteProductSize(id, sizeId);
            this.Context.SaveChanges();
            return JsonNetResult.Success();
        }



        // Images
        [HttpGet]
        public ActionResult Images(int id)
        {
            var result = this.ProductRepository.RetrieveImages(id);
            return new JsonNetResult()
            {
                Data = result,
                Formatting = Newtonsoft.Json.Formatting.Indented,
            };
        }

        [HttpPost]
        public ActionResult UpdateImageOrder(int id, string sorted)
        {
            this.ProductRepository.UpdateProductImageSort(id, sorted);
            return JsonNetResult.Success();
        }

        [HttpPost]
        public ActionResult ChangeImageColor(int id, int productImageId, int newColorId)
        {
            this.ProductRepository.ChangeImageColor(id, productImageId, newColorId);
            this.Context.SaveChanges();
            return JsonNetResult.Success();
        }
        
        [HttpPost]
        public ActionResult DeleteProductImage(int id, int productImageId)
        {
            this.ProductRepository.DeleteProductImage(id, productImageId);
            this.Context.SaveChanges();
            return JsonNetResult.Success();
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
            var result = this.ProductRepository.AddProductImage(id, image);
            this.Context.SaveChanges();
            return new JsonNetResult(result());
        }

        [HttpPost]
        public ActionResult AssignImagesToColor(int id)
        {
            this.ProductRepository.AssignImagesToColor(id);
            this.Context.SaveChanges();
            return JsonNetResult.Success();
        }

        [HttpPost]
        public ActionResult UnassignImagesFromColor(int id)
        {
            this.ProductRepository.UnassignImagesFromColor(id);
            this.Context.SaveChanges();
            return JsonNetResult.Success();
        }


    
        // Inventory
        [HttpGet]
        public ActionResult InventoryTotal(int id)
        {
            var result = this.InventoryRepository.TotalInStock(id);
            return new JsonNetResult(new { Total = result }); 
        }

        [HttpGet]
        public ActionResult Inventory(int id, bool regenerate = true)
        {
            if (regenerate)
            {
                //var total = this.ProductRepository.InventoryTotal(id);
                //if (total == 0)

                this.InventoryRepository.Generate(id);
                this.Context.SaveChanges();
            }

            var inventory = this.InventoryRepository.ProductSkuById(id);
            return new JsonNetResult(inventory); 
        }

        [HttpPost]
        public ActionResult Inventory(int id, int inventoryTotal)
        {
            this.InventoryRepository.UpdateInStock(id, inventoryTotal);
            this.Context.SaveChanges();
            return JsonNetResult.Success();
        }
    }
}