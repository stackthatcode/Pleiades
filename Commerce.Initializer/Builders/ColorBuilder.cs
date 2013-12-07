using System;
using System.Transactions;
using Commerce.Application.File;
using Commerce.Application.Lists;
using Commerce.Application.Lists.Entities;
using Pleiades.App.Data;
using Pleiades.App.Logging;
using Pleiades.App.Utility;
using DomainColor = Commerce.Application.Lists.Entities.Color;

namespace Commerce.Initializer.Builders
{
    public class ColorBuilder : IBuilder
    {
        private IUnitOfWork _unitOfWork;
        private IGenericRepository<DomainColor> _genericRepository;
        private IJsonColorRepository _colorRepository;
        private IImageBundleRepository _imageBundleRepository;
        
        public ColorBuilder(
            IUnitOfWork unitOfWork,
            IGenericRepository<DomainColor> genericRepository,
            IJsonColorRepository colorRepository,
            IImageBundleRepository imageBundleRepository)
        {
            _unitOfWork = unitOfWork;
            _genericRepository = genericRepository;
            _colorRepository = colorRepository;
            _imageBundleRepository = imageBundleRepository;
        }

        public void AddColor(JsonColor color, System.Drawing.Color rgbColor)
        {
            var imageBundle = _imageBundleRepository.AddColor(rgbColor);
            _unitOfWork.SaveChanges();
            color.ImageBundleExternalId = imageBundle.ExternalId.ToString();
            _colorRepository.Insert(color);
            _unitOfWork.SaveChanges();

        }

        public void Run()
        {
            using (var tx = new TransactionScope())
            {
                //_genericRepository.GetAll().ForEach(x => _genericRepository.Delete(x));
                //_unitOfWork.SaveChangesToDatabase();

                LoggerSingleton.Get().Info("Create the default Colors");

                AddColor(new JsonColor() { Name = "Red", SkuCode = "RED", SEO = "red"}, 
                    System.Drawing.Color.FromArgb(255, 0, 0));

                AddColor(new JsonColor() { Name = "Orange", SkuCode = "ORANGE", SEO = "orange" },
                    System.Drawing.Color.FromArgb(255, 144, 0));
                
                AddColor(new JsonColor() { Name = "White", SkuCode = "WHITE", SEO = "white" },
                    System.Drawing.Color.FromArgb(255, 255, 255));

                AddColor(new JsonColor() { Name = "Blue", SkuCode = "BLUE", SEO = "blue" },
                    System.Drawing.Color.FromArgb(0, 0, 255));

                AddColor(new JsonColor() { Name = "Black", SkuCode = "BLACK", SEO = "black" },
                    System.Drawing.Color.FromArgb(0, 0, 0));

                AddColor(new JsonColor() { Name = "Green", SkuCode = "GREEN", SEO = "green" },
                    System.Drawing.Color.FromArgb(0, 255, 0));                

                tx.Complete();
            }
        }
    }
}