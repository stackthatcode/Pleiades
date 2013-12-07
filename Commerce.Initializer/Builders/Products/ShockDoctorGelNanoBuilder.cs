using System;
using System.Transactions;
using Commerce.Application.File;
using Commerce.Application.Lists.Entities;
using Commerce.Application.Products;
using Commerce.Application.Products.Entities;
using Pleiades.App.Data;
using Pleiades.App.Logging;

namespace Commerce.Initializer.Builders.Products
{
    public class ShockDoctorGelNanoBuilder : ProductBuilder
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

        public ShockDoctorGelNanoBuilder(
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
                string name = "Shock Doctor Gel Nano Mouth Guard";
                LoggerSingleton.Get().Info("Creating Product: " + name);

                // Get reference data
                var brand = _brandRepository.FirstOrDefault(x => x.Name == "Shock Doctor");
                var category1 = _categoryRepository.FirstOrDefault(x => x.Name == "Mouth Guards");

                var product1 = new Product()
                {
                    Name = name,
                    Description = 
                        @"Protect the protector of your chops." +
                        @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                        @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco" +
                        @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt",
                    Synopsis = "Super Star BJJ Athletes need to wear this Gi.",
                    SEO = "SHOCKDOC-GELNANO",
                    SkuCode = "SHOCKDOC-GELNANO",
                    Active = true,
                    UnitPrice = 8.00m,
                    UnitCost = 1.00m,
                    Brand = brand,
                    Category = category1,
                    AssignImagesToColors = true,
                    IsDeleted = false,
                    DateCreated = DateTime.Now,
                    LastModified = DateTime.Now,
                };

                _genericProductRepository.Insert(product1);
                _unitOfWork.SaveChanges();

                var product1Id = product1.Id;
                var bundleList1 = AddImagesFromDirectory(@"Content\ShockDoctorGelNano");
                _unitOfWork.SaveChanges();

                bundleList1.ForEach(x => this.AddProductImage(product1Id, null, x));
                _unitOfWork.SaveChanges();

                _inventoryRepository.Generate(product1.Id);
                _unitOfWork.SaveChanges();

                var random = new Random();
                _inventoryRepository.RetreiveByProductId(product1.Id, false).ForEach(x =>
                {
                    x.Reserved = 0;
                    x.InStock = random.Next(0, 2);
                });
                _unitOfWork.SaveChanges();
                tx.Complete();
            }
        }
    }
}
