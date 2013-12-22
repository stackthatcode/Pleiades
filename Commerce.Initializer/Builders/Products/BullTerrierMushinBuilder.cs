﻿using System;
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
    public class BullTerrierMushinBuilder : ProductBuilder
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

        public BullTerrierMushinBuilder(
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
            string name = "Bull Terrier Mushin Gi";

            LoggerSingleton.Get().Info("Creating Product: " + name);

            // Get reference data
            var brand = _brandRepository.FirstOrDefault(x => x.Name == "Bull Terrier");
            var sizeGroup = _sizeGroupRepository.FirstOrDefault(x => x.Name == "Default Clothing");
            var black = _colorRepository.FirstOrDefault(x => x.SkuCode == "BLACK");
            var category1 = _categoryRepository.FirstOrDefault(x => x.Name == "Choke-proof Gis");

            var product1 = new Product()
            {
                Name = name,
                Description = "Spirit of Samurai - Mushin. Professional designed by Japanese Tattooer." +
                    @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                        @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco" +
                    @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt",
                Synopsis = "Spirit of Samurai - Mushin. Professional designed by Japanese Tattooer.",
                SEO = "bull-ter-mushin",
                SkuCode = "BULL-MUSHIN",
                Active = true,
                UnitPrice = 160.00m,
                UnitCost = 55.00m,
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

            var bundleList1 = AddImagesFromDirectory(@"Content\BullTerrierMuhsin\Black");
            _unitOfWork.SaveChanges();

            bundleList1.ForEach(x => this.AddProductImage(product1Id, productColor11().Id, x));
            _unitOfWork.SaveChanges();

            this.AddSizes(product1Id, sizeGroup);
            _unitOfWork.SaveChanges();

            _inventoryRepository.Generate(product1.Id);
            _unitOfWork.SaveChanges();

            var random = new Random();
            _inventoryRepository.RetreiveByProductId(product1.Id, false).ForEach(x =>
            {
                x.Reserved = 0;
                x.InStock = random.Next(0, 3);
            });
            _unitOfWork.SaveChanges();
        }
    }
}
