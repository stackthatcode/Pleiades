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
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Products;
using Commerce.Persist.Security;
using Commerce.WebUI;
using Commerce.WebUI.Plumbing;
using Color = Commerce.Domain.Model.Lists.Color;

namespace Commerce.Initializer.Builders
{
    public class ProductBuilder
    {
        public static void EmptyAndRepopulate(IContainerAdapter ServiceLocator)
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
                    SizeGroup = sizeGroup,
                };

                var productColor11 = new ProductColor { Color = black, Product = product1, Order = 1 };
                var productColor12 = new ProductColor { Color = blue, Product = product1, Order = 2 };

                var bundle11 = imageRepository.Add(ImageHelper("tat-1010_black_01_xl.jpg"));
                var bundle12 = imageRepository.Add(ImageHelper("tat-1010_black_02_xl.jpg"));
                var bundle13 = imageRepository.Add(ImageHelper("tat-1010_black_03_xl.jpg"));
                var bundle14 = imageRepository.Add(ImageHelper("tat-1010_black_04_xl.jpg"));
                var bundle15 = imageRepository.Add(ImageHelper("tat-1010_black_05_xl.jpg"));

                var productImage11 = new ProductImage { ProductColor = productColor11, ImageBundle = bundle11, Order = 1 };
                var productImage12 = new ProductImage { ProductColor = productColor11, ImageBundle = bundle12, Order = 2 };
                var productImage13 = new ProductImage { ProductColor = productColor11, ImageBundle = bundle13, Order = 3 };
                var productImage14 = new ProductImage { ProductColor = productColor11, ImageBundle = bundle14, Order = 4 };
                var productImage15 = new ProductImage { ProductColor = productColor11, ImageBundle = bundle15, Order = 5 };

                product1.Images.Add(productImage11);
                product1.Images.Add(productImage12);
                product1.Images.Add(productImage13);
                product1.Images.Add(productImage14);
                product1.Images.Add(productImage15);

                var bundle16 = imageRepository.Add(ImageHelper("tat-1010_blue_01_xl.jpg"));
                var bundle17 = imageRepository.Add(ImageHelper("tat-1010_blue_02_xl.jpg"));
                var bundle18 = imageRepository.Add(ImageHelper("tat-1010_blue_03_xl.jpg"));
                var bundle19 = imageRepository.Add(ImageHelper("tat-1010_blue_04_xl.jpg"));
                var bundle110 = imageRepository.Add(ImageHelper("tat-1010_blue_05_xl.jpg"));

                var productImage16 = new ProductImage { ProductColor = productColor12, ImageBundle = bundle16, Order = 1 };
                var productImage17 = new ProductImage { ProductColor = productColor12, ImageBundle = bundle17, Order = 2 };
                var productImage18 = new ProductImage { ProductColor = productColor12, ImageBundle = bundle18, Order = 3 };
                var productImage19 = new ProductImage { ProductColor = productColor12, ImageBundle = bundle19, Order = 4 };
                var productImage110 = new ProductImage { ProductColor = productColor12, ImageBundle = bundle110, Order = 5 };

                product1.Images.Add(productImage16);
                product1.Images.Add(productImage17);
                product1.Images.Add(productImage18);
                product1.Images.Add(productImage19);
                product1.Images.Add(productImage110);

                genericProductRepository.Insert(product1);
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
