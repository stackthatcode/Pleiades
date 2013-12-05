using System;
using Autofac;
using Commerce.Application.Lists;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Commerce.IntegrationTests.BuildersAndRepositories
{
    [TestFixture]
    public class BrandTests : FixtureBase
    {
        [Test]
        public void Empty_And_Repopulate_And_Update_Sizes()
        {
            using (var lifetime = TestContainer.LifetimeScope())
            {
                var brandRepository = lifetime.Resolve<IJsonBrandRepository>();
                lifetime.Resolve<BrandBuilder>().Run();

                var allBrands = brandRepository.RetrieveAll();                
                Console.WriteLine("\n");
                Console.WriteLine(JsonConvert.SerializeObject(allBrands, Formatting.Indented));
                Assert.That(allBrands.Count, Is.EqualTo(10));
            }
        }
    }
}
