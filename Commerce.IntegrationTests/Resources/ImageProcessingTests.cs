using System;
using System.Drawing;
using Autofac;
using NUnit.Framework;
using Pleiades.Application.Data;
using Commerce.Application.Interfaces;


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
                var image1 = new Bitmap(@".\TestImages\Image1.jpg");

                var imageBundleRepository = lifetime.Resolve<IImageBundleRepository>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();
                var resource1 = imageBundleRepository.AddBitmap(image1, true, true, false);

                Console.WriteLine(resource1.ExternalId);
                unitOfWork.SaveChanges();
            }
        }
    }
}
