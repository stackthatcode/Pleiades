using System;
using System.Linq;
using System.Transactions;
using Autofac;
using NUnit.Framework;
using Pleiades.Data;
using Pleiades.Data.EF;
using Pleiades.Injection;
using Pleiades.Utility;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Initializer.Builders;
using Commerce.Persist;
using Commerce.Persist.Security;
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
                var brandRepository = lifetime.Resolve<IBrandRepository>();
                    
                BrandBuilder.EmptyAndRepopulate(container);
                var allBrands = brandRepository.RetrieveAll();
                
                Console.WriteLine("\n");
                Console.WriteLine(JsonConvert.SerializeObject(allBrands, Formatting.Indented));
            }
        }
    }
}