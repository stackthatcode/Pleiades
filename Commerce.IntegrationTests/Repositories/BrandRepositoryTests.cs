using System;
using Autofac;
using Newtonsoft.Json;
using NUnit.Framework;
using Commerce.Application.Interfaces;
using Commerce.Initializer.Builders;

namespace Commerce.IntegrationTests.Repositories
{
    [TestFixture]
    public class BrandRepositoryTests : FixtureBase
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
            }
        }
    }
}