using System;
using System.Transactions;
using Commerce.Application.File;
using Commerce.Application.Lists.Entities;
using Commerce.Application.Products;
using Commerce.Application.Products.Entities;
using Pleiades.App.Data;
using Pleiades.App.Helpers;
using Pleiades.App.Logging;

namespace ArtOfGroundFighting.Initializer.Builders.Products
{
    public class TatamiEstiloBuilder : ProductBuilder
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<SizeGroup> _sizeGroupRepository;
        private readonly IGenericRepository<Product> _genericProductRepository;
        private readonly IGenericRepository<Color> _colorRepository;
        private readonly IGenericRepository<Brand> _brandRepository;
        private readonly IImageBundleRepository _imageRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TatamiEstiloBuilder(
                IGenericRepository<Category> categoryRepository, 
                IGenericRepository<SizeGroup> sizeGroupRepository, 
                IGenericRepository<Product> genericProductRepository, 
                IGenericRepository<Color> colorRepository, 
                IGenericRepository<Brand> brandRepository, 
                IImageBundleRepository imageRepository, 
                IProductRepository productRepository, 
                IInventoryRepository inventoryRepository, 
                IUnitOfWork unitOfWork) :
                    base(categoryRepository, sizeGroupRepository, genericProductRepository, colorRepository, 
                        brandRepository, imageRepository, productRepository, inventoryRepository, unitOfWork)
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

        public override void Run()
        {            
            using (var tx = new TransactionScope())
            {
                string name = "Tatami Estilo 3.0 Premier BJJ Gi";

                LoggerSingleton.Get().Info("Creating Product: " + name);

                // Get reference data
                var brandTatami = _brandRepository.FirstOrDefault(x => x.Name == "Tatami");
                var sizeGroup = _sizeGroupRepository.FirstOrDefault(x => x.Name == "Default Clothing");
                var black = _colorRepository.FirstOrDefault(x => x.SkuCode == "BLACK");
                var blue = _colorRepository.FirstOrDefault(x => x.SkuCode == "BLUE");
                var category1 = _categoryRepository.FirstOrDefault(x => x.Name == "Choke-proof Gis");

                var product1 = new Product()
                {
                    Name = name,
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

                var bundleList1 = AddImagesFromDirectory(@"Content\TatamiEstilo\Black");
                _unitOfWork.SaveChanges();

                bundleList1.ForEach(x => this.AddProductImage(product1Id, productColor11().Id, x));
                _unitOfWork.SaveChanges();

                var bundleList2 = AddImagesFromDirectory(@"Content\TatamiEstilo\Blue");
                _unitOfWork.SaveChanges();

                bundleList2.ForEach(x => this.AddProductImage(product1Id, productColor12().Id, x));
                _unitOfWork.SaveChanges();

                this.AddSizes(product1Id, sizeGroup);
                _unitOfWork.SaveChanges();

                _inventoryRepository.Generate(product1.Id);
                _unitOfWork.SaveChanges();

                var random = new Random();
                _inventoryRepository.RetreiveByProductId(product1.Id, false).ForEach(x =>
                {
                    x.Reserved = 0;
                    x.InStock = random.Next(5, 10);
                });
                _unitOfWork.SaveChanges();
                tx.Complete();
            }
        }
    }
}
