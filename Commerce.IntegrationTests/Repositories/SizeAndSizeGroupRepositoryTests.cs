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
    public class SizeAndSizeGroupRepositoryTests
    {       
        [Test]
        public void DoStuffWithCategoriesAndBeHappy()
        {
            // Gotta do this
            FixtureBase.TriggerMe();

            // Empty + populate data using the Initializer Builder
            var container = TestContainer.LifetimeScope().Resolve<IContainerAdapter>();
            SizeBuilder.EmptyAndRepopulate(container);

            using (var lifetime = TestContainer.LifetimeScope())
            {
                var repository = lifetime.Resolve<ISizeGroupRepository>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

                var results = repository.GetAllJson();
                var sz = JsonConvert.SerializeObject(results);
                Console.WriteLine(sz);
            }
        }
    }
}
