using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Commerce.Application.Model.Resources;
using Pleiades.Application.Data;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Lists;
using Commerce.Application.Model.Products;
using Pleiades.Application.Logging;
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

        public virtual void Run()
        {
        }

        public void AddSizes(int productId, SizeGroup sizeGroup)
        {
            foreach (var size in sizeGroup.Sizes)
            {
                _productRepository.AddProductSize(productId, size.ID);
            }            
        }

        public List<ImageBundle> AddImagesFromDirectory(string directory)
        {
            var output = new List<ImageBundle>();
            foreach (var file in Directory.GetFiles(directory))
            {
                var path = Path.Combine(directory, file);
                LoggerSingleton.Get().Info("Adding Image for " + file);
                output.Add(AddImage(file));
            }
            return output;
        }

        public ImageBundle AddImage(string filePath)
        {
            var rawImage = new Bitmap(filePath);
            return _imageRepository.AddBitmap(rawImage, true, true, false);
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
    }
}
