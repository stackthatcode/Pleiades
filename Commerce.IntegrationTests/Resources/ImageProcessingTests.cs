﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Autofac;
using NUnit.Framework;
using Pleiades.Application;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Resources;
using Pleiades.Application.Data;


namespace Commerce.IntegrationTests.Resources
{
    [TestFixture]
    public class ImageProcessingTests : FixtureBase
    {
        [Test]
        public void UploadImagesToFileRepository()
        {
            using (var lifetime = TestContainer.LifetimeScope())
            {
                var currentDirectory = Environment.CurrentDirectory;
                var image1 = new Bitmap(@".\TestImages\Image1.jpg");

                var imageBundleRepository = lifetime.Resolve<IImageBundleRepository>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();
                var resource1 = imageBundleRepository.Add(image1);

                Console.WriteLine(resource1.ExternalId);
                unitOfWork.SaveChanges();
            }
        }
    }
}
