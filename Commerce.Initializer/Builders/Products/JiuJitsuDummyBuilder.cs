﻿using System;
using System.Transactions;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Lists;
using Commerce.Application.Model.Products;
using Pleiades.Application.Data;
using Pleiades.Application.Logging;

namespace Commerce.Initializer.Builders.Products
{
    public class JiuJitsuDummyBuilder : ProductBuilder
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

        public JiuJitsuDummyBuilder(
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
                string name = "Ring-to-Cage Jiu-Jitsu Training Dummy";
                LoggerSingleton.Get().Info("Creating Product: " + name);

                // Get reference data
                var brand = _brandRepository.FirstOrDefault(x => x.SkuCode == "RINGTOCAGE");
                var black = _colorRepository.FirstOrDefault(x => x.SkuCode == "BLACK");
                var category1 = _categoryRepository.FirstOrDefault(x => x.Name == "Training Dummies");

                var product1 = new Product()
                {
                    Name = name,
                    Description = "Train even without a partner." +
                        @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                        @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco" +
                        @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt",
                    Synopsis = "Super Star BJJ Athletes need to wear this Gi.",
                    SEO = "RINGTOCAGE-DUMMY",
                    SkuCode = "RINGTOCAGE-DUMMY",
                    Active = true,
                    UnitPrice = 295.00m,
                    UnitCost = 160.00m,
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

                var productColor11 = this.AddProductColor(product1Id, black);
                _unitOfWork.SaveChanges();

                var bundleList1 = AddImagesFromDirectory(@"Content\JiuJitsuDummy");
                _unitOfWork.SaveChanges();

                bundleList1.ForEach(x => this.AddProductImage(product1Id, productColor11().Id, x));
                _unitOfWork.SaveChanges();

                _inventoryRepository.Generate(product1.Id);
                _unitOfWork.SaveChanges();

                var random = new Random();
                _inventoryRepository.RetreiveByProductId(product1.Id, false).ForEach(x =>
                {
                    x.Reserved = 0;
                    x.InStock = random.Next(0, 1);
                });
                _unitOfWork.SaveChanges();
                tx.Complete();
            }
        }
    }
}
