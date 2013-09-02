using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Transactions;
using Commerce.Application.Model.Resources;
using Pleiades.Application.Data;
using Pleiades.Application.Logging;
using Pleiades.Application.Utility;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Lists;
using Commerce.Application.Model.Products;
using Color = Commerce.Application.Model.Lists.Color;

namespace Commerce.Initializer.Builders
{
    public class ProductBuilder : IBuilder
    {
        IGenericRepository<Category> _categoryRepository;
        IGenericRepository<SizeGroup> _sizeGroupRepository;
        IGenericRepository<Product> _genericProductRepository;
        IGenericRepository<Color> _colorRepository;
        IGenericRepository<Brand> _brandRepository;
        IImageBundleRepository _imageRepository;
        IProductRepository _productRepository;
        IInventoryRepository _inventoryRepository;
        IUnitOfWork _unitOfWork;
        
        public ProductBuilder(
            IGenericRepository<Category> categoryRepository,
            IGenericRepository<SizeGroup> sizeGroupRepository,
            IGenericRepository<Product> genericProductRepository,
            IGenericRepository<Color> colorRepository,
            IGenericRepository<Brand> brandRepository,
            IImageBundleRepository imageRepository,
            IProductRepository productRepository,
            IInventoryRepository inventoryRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _sizeGroupRepository = sizeGroupRepository;
            _genericProductRepository = genericProductRepository;
            _colorRepository = colorRepository;
            _brandRepository = brandRepository;
            _imageRepository = imageRepository;
            _productRepository = productRepository;
            _inventoryRepository = inventoryRepository;
            _unitOfWork = unitOfWork;
        }

        public void Run()
        {
            using (var tx = new TransactionScope())
            {
                LoggerSingleton.Get().Info("Create the default Products");

                // Clear everything out
                _genericProductRepository.GetAll().ForEach(x => _genericProductRepository.Delete(x));
                _unitOfWork.SaveChanges();

                // Get reference data
                var brandTatami = _brandRepository.GetAll().ToList()[4];     // TATAMI
                var sizeGroup = _sizeGroupRepository.GetAll().ToList()[1];   // SIZE GROUP
                var black = _colorRepository.FirstOrDefault(x => x.SkuCode == "BLACK");
                var blue = _colorRepository.FirstOrDefault(x => x.SkuCode == "BLUE");
                var category1 = _categoryRepository.FirstOrDefault(x => x.Name == "Choke-proof Gis");

                // Create Product 1 - Image Bundles
                var product1 = new Product()
                {
                    Name = "Tatami Estilo 3.0 Premier BJJ Gi",
                    Description = "The Tatami Fightwear Estilo Premier BJJ GI range is designed for the BJJ athlete who is looking for a " +
                        "BJJ GI that is built to the highest quality and craftsmanship but also with cutting edge style and detailing. " +
                        "The Estilo BJJ GI is constructed using only the best quality materials and is a must for any serious BJJ athletes.",
                    Synopsis = "The Tatami Fightwear Estilo Premier BJJ GI range is designed for the BJJ athlete that's tough!",
                    SEO = "tatmi-estilo",
                    SkuCode = "TAT-1010",
                    Active = true,
                    UnitPrice = 130.00m,
                    UnitCost = 45.00m,
                    Brand = brandTatami,
                    Category = category1,
                    AssignImagesToColors = true,
                    IsDeleted = false,
                    DateCreated = DateTime.Now,
                    LastModified = DateTime.Now,
                };
                _genericProductRepository.Insert(product1);
                _unitOfWork.SaveChanges();
                var product1Id = product1.Id;

                var productColor11 = this.AddProductColor(product1Id, black);
                var productColor12 = this.AddProductColor(product1Id, blue);
                _unitOfWork.SaveChanges();
                
                var bundle11 = AddImage("tat-1010_black_01_xl.jpg");
                var bundle12 = AddImage("tat-1010_black_02_xl.jpg");
                var bundle13 = AddImage("tat-1010_black_03_xl.jpg");
                var bundle14 = AddImage("tat-1010_black_04_xl.jpg");
                var bundle15 = AddImage("tat-1010_black_05_xl.jpg");
                _unitOfWork.SaveChanges();

                this.AddProductImage(product1Id, productColor11().Id, bundle11);
                this.AddProductImage(product1Id, productColor11().Id, bundle12);
                this.AddProductImage(product1Id, productColor11().Id, bundle13);
                this.AddProductImage(product1Id, productColor11().Id, bundle14);
                this.AddProductImage(product1Id, productColor11().Id, bundle15);

                var bundle16 = AddImage("tat-1010_blue_01_xl.jpg");
                var bundle17 = AddImage("tat-1010_blue_02_xl.jpg");
                var bundle18 = AddImage("tat-1010_blue_03_xl.jpg");
                var bundle19 = AddImage("tat-1010_blue_04_xl.jpg");
                var bundle110 = AddImage("tat-1010_blue_05_xl.jpg");
                _unitOfWork.SaveChanges();

                this.AddProductImage(product1Id, productColor12().Id, bundle16);
                this.AddProductImage(product1Id, productColor12().Id, bundle17);
                this.AddProductImage(product1Id, productColor12().Id, bundle18);
                this.AddProductImage(product1Id, productColor12().Id, bundle19);
                this.AddProductImage(product1Id, productColor12().Id, bundle110);
                _unitOfWork.SaveChanges();

                this.AddSizes(product1Id, sizeGroup);
                _unitOfWork.SaveChanges();

                _inventoryRepository.Generate(product1.Id);
                _unitOfWork.SaveChanges();

                var random = new Random();
                _inventoryRepository.ProductSkuById(product1.Id).ForEach(x =>
                    {
                        x.Reserved = 0;
                        x.InStock = random.Next(2, 6);
                    });
                _unitOfWork.SaveChanges();
                tx.Complete();
            }
        }

        public void AddSizes(int productId, SizeGroup sizeGroup)
        {
            foreach (var size in sizeGroup.Sizes)
            {
                _productRepository.AddProductSize(productId, size.ID);
            }            
        }

        public ImageBundle AddImage(string filename)
        {
            return _imageRepository.Add(ImageHelper(filename));
        }

        public JsonProductImage AddProductImage(int productId, int? colorId, ImageBundle imageBundle)
        {
            return _productRepository.AddProductImage(
                productId,
                new JsonProductImage
                {
                    ImageBundleExternalId = imageBundle.ExternalId.ToString(),
                    ProductColorId = colorId.Value,
                })();          
        }

        public Func<JsonProductColor> AddProductColor(int productId, Color color)
        {
            return _productRepository.AddProductColor(productId, color.Id);
        }

        static Bitmap ImageHelper(string filename)
        {
            return new Bitmap(Path.Combine(BrandLogoDirectory(), filename));
        }

        static string BrandLogoDirectory()
        {
            return ConfigurationManager.AppSettings["DefaultBrandLogos"];
        }
    }
}
