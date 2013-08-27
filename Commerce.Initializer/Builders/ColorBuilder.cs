using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Transactions;
using Pleiades.Application.Data;
using Pleiades.Application.Injection;
using Pleiades.Application;
using Pleiades.Application.Utility;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Providers;
using Commerce.Persist.Concrete;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Lists;
using DomainColor = Commerce.Persist.Model.Lists.Color;
using Commerce.Persist.Model.Resources;
using Commerce.Persist;
using Commerce.Web;
using Commerce.Web.Plumbing;


namespace Commerce.Initializer.Builders
{
    public static class ColorBuilder
    {
        public static void Populate(IContainerAdapter ServiceLocator)
        {
            using (var tx = new TransactionScope())
            {
                Console.WriteLine("Create the default Colors");

                var unitOfWork = ServiceLocator.Resolve<IUnitOfWork>();
                var genericRepository = ServiceLocator.Resolve<IGenericRepository<DomainColor>>();
                var colorRepository = ServiceLocator.Resolve<IJsonColorRepository>();
                var imageBundleRepository = ServiceLocator.Resolve<IImageBundleRepository>();

                genericRepository.GetAll().ForEach(x => genericRepository.Delete(x));
                unitOfWork.SaveChanges();


                // *** Red *** //
                var imageBundle1 = imageBundleRepository.Add(System.Drawing.Color.FromArgb(255, 0, 0), 150, 150);
                unitOfWork.SaveChanges();
                var red = new JsonColor()
                {
                    Name = "Red",
                    SkuCode = "RED",
                    SEO = "red",
                    ImageBundleExternalId = imageBundle1.ExternalId.ToString(),
                };
                colorRepository.Insert(red);
                unitOfWork.SaveChanges();


                // *** White *** //
                var imageBundle2 = imageBundleRepository.Add(System.Drawing.Color.FromArgb(255, 255, 255), 150, 150);
                unitOfWork.SaveChanges();
                var white = new JsonColor()
                {
                    Name = "White",
                    SkuCode = "WHITE",
                    SEO = "white",
                    ImageBundleExternalId = imageBundle2.ExternalId.ToString(),
                };
                colorRepository.Insert(white);
                unitOfWork.SaveChanges();


                // *** Blue *** //
                var imageBundle3 = imageBundleRepository.Add(System.Drawing.Color.FromArgb(0, 0, 255), 150, 150);
                unitOfWork.SaveChanges();
                var blue = new JsonColor()
                {
                    Name = "Blue",
                    SkuCode = "BLUE",
                    SEO = "blue",
                    ImageBundleExternalId = imageBundle3.ExternalId.ToString(),
                };
                colorRepository.Insert(blue);
                unitOfWork.SaveChanges();


                // *** Black *** //
                var imageBundle4 = imageBundleRepository.Add(System.Drawing.Color.FromArgb(0, 0, 0), 150, 150);
                unitOfWork.SaveChanges();
                var black = new JsonColor()
                {
                    Name = "Black",
                    SkuCode = "BLACK",
                    SEO = "black",
                    ImageBundleExternalId = imageBundle4.ExternalId.ToString(),
                };
                colorRepository.Insert(black);
                unitOfWork.SaveChanges();


                // *** Green *** //
                var imageBundle5 = imageBundleRepository.Add(System.Drawing.Color.FromArgb(0, 255, 0), 150, 150);
                unitOfWork.SaveChanges();
                var green = new JsonColor()
                {
                    Name = "Green",
                    SkuCode = "GREEN",
                    SEO = "green",
                    ImageBundleExternalId = imageBundle5.ExternalId.ToString(),
                };
                colorRepository.Insert(green);
                unitOfWork.SaveChanges();

                tx.Complete();
            }
        }
    }
}