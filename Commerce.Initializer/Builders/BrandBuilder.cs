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
using Commerce.Persist.Model.Resources;
using Commerce.Web;
using Commerce.Web.Plumbing;


namespace Commerce.Initializer.Builders
{
    public static class BrandBuilder
    {
        public static void Populate(IContainerAdapter ServiceLocator)
        {
            using (var tx = new TransactionScope())
            {
                Console.WriteLine("Create the default Brands");

                var unitOfWork = ServiceLocator.Resolve<IUnitOfWork>();
                var genericRepository = ServiceLocator.Resolve<IGenericRepository<Brand>>();
                var brandRepository = ServiceLocator.Resolve<IJsonBrandRepository>();
                var imageBundleRepository = ServiceLocator.Resolve<IImageBundleRepository>();

                genericRepository.GetAll().ForEach(x => genericRepository.Delete(x));
                unitOfWork.SaveChanges();

                // *** Affliction *** //
                var imageBundle1 = imageBundleRepository.Add(new Bitmap(Path.Combine(BrandLogoDirectory(), "Afflictionmma2.jpg")));
                unitOfWork.SaveChanges();
                var brand1 = new JsonBrand()
                {
                    Name = "Affliction",
                    Description = 
                        @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                        @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco",
                    SEO = "afflication-mma",
                    SkuCode = "AFFL",
                    ImageBundleExternalId = imageBundle1.ExternalId.ToString(),
                };
                var result1 = brandRepository.Insert(brand1);
                unitOfWork.SaveChanges();

                // *** Bad Boy *** //
                var imageBundle2 = imageBundleRepository.Add(new Bitmap(Path.Combine(BrandLogoDirectory(), "badboy.jpg")));
                unitOfWork.SaveChanges();
                var brand2 = new JsonBrand()
                {
                    Name = "Bad Boy",
                    Description = @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                        @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco",
                    SEO = "bad-boy-mma",
                    SkuCode = "BBOY",
                    ImageBundleExternalId = imageBundle2.ExternalId.ToString(),
                };
                var result2 = brandRepository.Insert(brand2);
                unitOfWork.SaveChanges();

                // *** Dethrone *** //
                var imageBundle3 = imageBundleRepository.Add(new Bitmap(Path.Combine(BrandLogoDirectory(), "dethrone2.png")));
                unitOfWork.SaveChanges();
                var brand3 = new JsonBrand()
                {
                    Name = "Dethrone",
                    Description = @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                        @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco",
                    SEO = "dethrone-mma",
                    SkuCode = "DETHRONE",
                    ImageBundleExternalId = imageBundle3.ExternalId.ToString(),
                };
                var result3 = brandRepository.Insert(brand3);
                unitOfWork.SaveChanges();

                // *** Fuji *** //
                var imageBundle4 = imageBundleRepository.Add(new Bitmap(Path.Combine(BrandLogoDirectory(), "fuji.jpg")));
                unitOfWork.SaveChanges();
                var brand4 = new JsonBrand()
                {
                    Name = "Fuji",
                    Description = @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                        @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco",
                    SEO = "fuji-mma",
                    SkuCode = "FUJI",
                    ImageBundleExternalId = imageBundle4.ExternalId.ToString(),
                };
                var result4 = brandRepository.Insert(brand4);
                unitOfWork.SaveChanges();

                // *** Tatami *** //
                var imageBundle5 = imageBundleRepository.Add(new Bitmap(Path.Combine(BrandLogoDirectory(), "tatami.jpg")));
                unitOfWork.SaveChanges();
                var brand5 = new JsonBrand()
                {
                    Name = "Tatami",
                    Description = @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                        @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco",
                    SEO = "tatami-mma",
                    SkuCode = "TATAMI",
                    ImageBundleExternalId = imageBundle5.ExternalId.ToString(),
                };
                var result5 = brandRepository.Insert(brand5);
                unitOfWork.SaveChanges();

                tx.Complete();
            }

        }

        public static string BrandLogoDirectory()
        {
            return ConfigurationManager.AppSettings["DefaultBrandLogos"];
        }
    }
}