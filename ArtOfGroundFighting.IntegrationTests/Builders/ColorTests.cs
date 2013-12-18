using System;
using ArtOfGroundFighting.IntegrationTests;
using Autofac;
using Commerce.Application.Lists;
using ArtOfGroundFighting.Initializer.Builders;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ArtOfGroundFighting.IntegrationTests.Builders
{
    [TestFixture]
    public class ColorTests : FixtureBase
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
