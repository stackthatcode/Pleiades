using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Transactions;
using Pleiades.Data;
using Pleiades.Injection;
using Pleiades.Utility;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Providers;
using Commerce.Persist.Concrete;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Lists;
using Commerce.Persist.Model.Products;
using Commerce.Web;
using Commerce.Web.Plumbing;
using Color = Commerce.Persist.Model.Lists.Color;

namespace Commerce.Initializer.Builders
{
    public class ProductBuilder
    {
        public static void Populate(IContainerAdapter ServiceLocator)
        {
            using (var tx = new TransactionScope())
            {
                Console.WriteLine("Create the default Products");

                // Create the Repos
                var categoryRepository = ServiceLocator.Resolve<IGenericRepository<Category>>();
                var sizeGroupRepository = ServiceLocator.Resolve<IGenericRepository<SizeGroup>>();
                var genericProductRepository = ServiceLocator.Resolve<IGenericRepository<Product>>();
                var colorRepository = ServiceLocator.Resolve<IGenericRepository<Color>>();
                var brandRepository = ServiceLocator.Resolve<IGenericRepository<Brand>>();
                var imageRepository = ServiceLocator.Resolve<IImageBundleRepository>();
                var productRepository = ServiceLocator.Resolve<IProductRepository>();
                var productColorRepository = ServiceLocator.Resolve<IProductRepository>();
                var inventoryRepository = ServiceLocator.Resolve<IInventoryRepository>();
                var unitOfWork = ServiceLocator.Resolve<IUnitOfWork>();

                // Clear everything out
                genericProductRepository.GetAll().ForEach(x => genericProductRepository.Delete(x));
                unitOfWork.SaveChanges();

                // Get reference data
                var brandTatami = brandRepository.GetAll().ToList()[4];     // TATAMI
                var sizeGroup = sizeGroupRepository.GetAll().ToList()[1];   // SIZE GROUP
                var black = colorRepository.FirstOrDefault(x => x.SkuCode == "BLACK");
                var blue = colorRepository.FirstOrDefault(x => x.SkuCode == "BLUE");
                var category1 = categoryRepository.FirstOrDefault(x => x.Name == "Choke-proof Gis");

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
                genericProductRepository.Insert(product1);
                unitOfWork.SaveChanges();
                var product1Id = product1.Id;

                var productColor12Result = productRepository.AddProductColor(product1Id, blue.Id);
                var productColor11Result = productRepository.AddProductColor(product1Id, black.Id);
                unitOfWork.SaveChanges();
                
                var bundle11 = imageRepository.Add(ImageHelper("tat-1010_black_01_xl.jpg"));
                var bundle12 = imageRepository.Add(ImageHelper("tat-1010_black_02_xl.jpg"));
                var bundle13 = imageRepository.Add(ImageHelper("tat-1010_black_03_xl.jpg"));
                var bundle14 = imageRepository.Add(ImageHelper("tat-1010_black_04_xl.jpg"));
                var bundle15 = imageRepository.Add(ImageHelper("tat-1010_black_05_xl.jpg"));
                unitOfWork.SaveChanges();

                productRepository.AddProductImage(product1Id, new JsonProductImage
                {
                    ImageBundleExternalId = bundle11.ExternalId.ToString(),
                    ProductColorId = productColor11Result().Id,
                    Order = 1
                });
                productRepository.AddProductImage(product1Id, new JsonProductImage
                {
                    ImageBundleExternalId = bundle12.ExternalId.ToString(),
                    ProductColorId = productColor11Result().Id,
                });
                productRepository.AddProductImage(product1Id, new JsonProductImage
                {
                    ImageBundleExternalId = bundle13.ExternalId.ToString(),
                    ProductColorId = productColor11Result().Id,
                });
                productRepository.AddProductImage(product1Id, new JsonProductImage
                {
                    ImageBundleExternalId = bundle14.ExternalId.ToString(),
                    ProductColorId = productColor11Result().Id,
                });
                productRepository.AddProductImage(product1Id, new JsonProductImage
                {
                    ImageBundleExternalId = bundle15.ExternalId.ToString(),
                    ProductColorId = productColor11Result().Id,
                });

                var bundle16 = imageRepository.Add(ImageHelper("tat-1010_blue_01_xl.jpg"));
                var bundle17 = imageRepository.Add(ImageHelper("tat-1010_blue_02_xl.jpg"));
                var bundle18 = imageRepository.Add(ImageHelper("tat-1010_blue_03_xl.jpg"));
                var bundle19 = imageRepository.Add(ImageHelper("tat-1010_blue_04_xl.jpg"));
                var bundle110 = imageRepository.Add(ImageHelper("tat-1010_blue_05_xl.jpg"));
                unitOfWork.SaveChanges();

                productRepository.AddProductImage(product1Id, new JsonProductImage
                {
                    ImageBundleExternalId = bundle16.ExternalId.ToString(),
                    ProductColorId = productColor12Result().Id,
                });
                productRepository.AddProductImage(product1Id, new JsonProductImage
                {
                    ImageBundleExternalId = bundle17.ExternalId.ToString(),
                    ProductColorId = productColor12Result().Id,
                });
                productRepository.AddProductImage(product1Id, new JsonProductImage
                {
                    ImageBundleExternalId = bundle18.ExternalId.ToString(),
                    ProductColorId = productColor12Result().Id,
                });
                productRepository.AddProductImage(product1Id, new JsonProductImage
                {
                    ImageBundleExternalId = bundle19.ExternalId.ToString(),
                    ProductColorId = productColor12Result().Id,
                });

                productRepository.AddProductImage(product1Id, new JsonProductImage
                {
                    ImageBundleExternalId = bundle110.ExternalId.ToString(),
                    ProductColorId = productColor12Result().Id,
                });
                unitOfWork.SaveChanges();

                foreach (var size in sizeGroup.Sizes)
                {
                    productRepository.AddProductSize(product1.Id, size.ID);
                }
                unitOfWork.SaveChanges();

                inventoryRepository.Generate(product1.Id);
                unitOfWork.SaveChanges();

                var random = new Random();
                foreach (var sku in inventoryRepository.ProductSkuById(product1.Id))
                {
                    sku.Reserved = 0;
                    sku.InStock = random.Next(2, 6);
                }
                unitOfWork.SaveChanges();
                tx.Complete();
            }
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
