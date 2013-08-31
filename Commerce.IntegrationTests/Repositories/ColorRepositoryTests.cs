using System;
using Autofac;
using Newtonsoft.Json;
using NUnit.Framework;
using Commerce.Application.Interfaces;
using Commerce.Initializer.Builders;

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
                // Empty + populate data using the Initializer Builder
                lifetime.Resolve<ColorBuilder>().Run();
                var colorRepository = lifetime.Resolve<IJsonColorRepository>();

                var colors = colorRepository.RetrieveAll();
                Console.WriteLine("\n");
                Console.WriteLine(JsonConvert.SerializeObject(colors, Formatting.Indented));
            }
        }
    }
}
