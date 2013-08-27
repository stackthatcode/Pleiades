using System;
using System.Linq;
using System.Transactions;
using Autofac;
using NUnit.Framework;
using Pleiades.Application.Injection;
using Pleiades.Application;
using Pleiades.Application.EF;
using Pleiades.Application.Utility;
using Commerce.Persist;
using Commerce.Persist.Concrete;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Lists;
using Commerce.Initializer.Builders;
using Newtonsoft.Json;

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
                var container = lifetime.Resolve<IContainerAdapter>();
                var brandRepository = lifetime.Resolve<IJsonBrandRepository>();
                    
                BrandBuilder.Populate(container);
                var allBrands = brandRepository.RetrieveAll();
                
                Console.WriteLine("\n");
                Console.WriteLine(JsonConvert.SerializeObject(allBrands, Formatting.Indented));
            }
        }
    }
}