using System.Drawing;
using System.IO;
using Autofac;
using Commerce.Application.File;
using NUnit.Framework;

namespace Commerce.IntegrationTests.Azure
{
    [TestFixture]
    [Explicit]
    public class TestAzureFileRepository
    {
        [Test]
        public void SaveFileToBlobStorage()
        {
            using (var lifetime = TestContainer.AzureLifetimeScope())
            {
                var repository = lifetime.Resolve<IFileResourceRepository>();
                var bitmap = new Bitmap(@"Azure\Afflictionmma2.jpg");
                var entity = repository.AddNew(bitmap);

                var roundTripBytes = repository.RetrieveBytes(entity.ExternalId);
                File.WriteAllBytes(@"Azure\TestAzureImage.png", roundTripBytes);
            }
        }
    }
}
