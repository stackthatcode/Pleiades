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
    public class ColorRepositoryTests : FixtureBase
    {
        [Test]
        public void Empty_And_Repopulate_And_Update_Sizes()
        {
            using (var lifetime = TestContainer.LifetimeScope())
            {
                var container = lifetime.Resolve<IContainerAdapter>();
                var colorRepository = lifetime.Resolve<IJsonColorRepository>();

                ColorBuilder.Populate(container);
                var colors = colorRepository.RetrieveAll();

                Console.WriteLine("\n");
                Console.WriteLine(JsonConvert.SerializeObject(colors, Formatting.Indented));
            }
        }
    }
}
