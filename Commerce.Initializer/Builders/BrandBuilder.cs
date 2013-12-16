using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Transactions;
using Commerce.Application.File;
using Commerce.Application.Lists;
using Commerce.Application.Lists.Entities;
using Pleiades.App.Data;
using Pleiades.App.Logging;
using Pleiades.App.Utility;


namespace ArtOfGroundFighting.Initializer.Builders
{
    public class BrandBuilder : IBuilder
    {
        public IUnitOfWork _unitOfWork;
        public IGenericRepository<Brand> _genericRepository;
        public IJsonBrandRepository _brandRepository;
        public IImageBundleRepository _imageBundleRepository;
        
        public BrandBuilder(
                IUnitOfWork unitOfWork,
                IGenericRepository<Brand> genericRepository,
                IJsonBrandRepository brandRepository,
                IImageBundleRepository imageBundleRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
            _brandRepository = brandRepository;
            _imageBundleRepository = imageBundleRepository;
        }

        public void AddBrand(string brandImage, string name, string descrtipion, string SEO, string SkuCode)
        {
            LoggerSingleton.Get().Info("brandImage File Path - " + brandImage);
            var imageBundle1 = _imageBundleRepository.AddBitmap(new Bitmap(brandImage), false, false, false);
            _unitOfWork.SaveChanges();
            var brand1 = new JsonBrand()
            {
                Name = name,
                Description = descrtipion,
                SEO = SEO,
                SkuCode = SkuCode,
                ImageBundleExternalId = imageBundle1.ExternalId.ToString(),
            };
            _brandRepository.Insert(brand1);
            _unitOfWork.SaveChanges();            
        }

        public string BrandLogoDirectory()
        {
            return @"Content\BrandLogos";
        }

        public void Run()
        {
            using (var tx = new TransactionScope())
            {
                LoggerSingleton.Get().Info("Create the default Brands");

                //_genericRepository.GetAll().ForEach(x => _genericRepository.Delete(x));
                //_unitOfWork.SaveChangesToDatabase();

                AddBrand(Path.Combine(BrandLogoDirectory(), "Afflictionmma2.jpg"),
                         "Affliction",
                         @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                         @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco",
                         "afflication-mma",
                         "AFFL");

                AddBrand(Path.Combine(BrandLogoDirectory(), "badboy.jpg"),
                         "Bad Boy",
                         @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                         @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco",
                         "bad-boy-mma",
                         "BBOY");

                AddBrand(Path.Combine(BrandLogoDirectory(), "bullterriergearl.gif"),
                         "Bull Terrier",
                         @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                         @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco",
                         "bull-terrier-mma",
                         "BBOY");

                AddBrand(Path.Combine(BrandLogoDirectory(), "dethrone2.png"),
                         "Dethrone",
                         @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                         @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco",
                         "dethrone-mma",
                         "DETHRONE");

                AddBrand(Path.Combine(BrandLogoDirectory(), "fuji.jpg"),
                         "Fuji",
                         @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                         @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco",
                         "fuji-mma",
                         "FUJI");

                AddBrand(Path.Combine(BrandLogoDirectory(), "tatami.jpg"),
                         "Tatami",
                         @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                         @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco",
                         "tatami-mma",
                         "TATAMI");

                AddBrand(Path.Combine(BrandLogoDirectory(), "hayabusa-logo.jpg"),
                         "Hayabusa",
                         @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                         @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco",
                         "tatami-mma",
                         "HAYABUSA");

                AddBrand(Path.Combine(BrandLogoDirectory(), "RingToCageLogo.jpg"),
                         "Ring to Cage",
                         @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                         @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco",
                         "tatami-mma",
                         "RINGTOCAGE");

                AddBrand(Path.Combine(BrandLogoDirectory(), "shock_doctor_logo.jpg"),
                         "Shock Doctor",
                         @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                         @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco",
                         "shock-doctor",
                         "SHOCKDOCTOR");

                AddBrand(Path.Combine(BrandLogoDirectory(), "logotapout.jpg"),
                         "Tapout",
                         @"Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt" +
                         @"ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco",
                         "tapout",
                         "TAPOUT");

                tx.Complete();
            }
        }
    }
}