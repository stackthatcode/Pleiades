﻿using Autofac;
using NUnit.Framework;
using Pleiades.Application.Data;
using Pleiades.Application.Injection;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Lists;
using Commerce.Application.Model.Products;
using Commerce.Initializer.Builders;

namespace Commerce.IntegrationTests.Repositories
{
    [TestFixture]
    public class ProductRepositoryTests : FixtureBase
    {
        [Test]
        public void Empty_And_Repopulate_And_Update_Products()
        {
            using (var lifetime = TestContainer.LifetimeScope())
            {
                var categoryRepository = lifetime.Resolve<IGenericRepository<Category>>();
                var productSearchRepository = lifetime.Resolve<IProductRepository>();

                lifetime.Resolve<BrandBuilder>().Run();
                lifetime.Resolve<ColorBuilder>().Run();
                lifetime.Resolve<SizeBuilder>().Run();
                lifetime.Resolve<CategoryBuilder>().Run();
                lifetime.Resolve<ProductBuilder>().Run();

                // Test the search query
                var category1 = categoryRepository.FirstOrDefault(x => x.Name == "Jiu-jitsu Gear");
                var category2 = categoryRepository.FirstOrDefault(x => x.Name == "Choke-proof Gis");
                var category3 = categoryRepository.FirstOrDefault(x => x.Name == "Shoes");

                var search1 = productSearchRepository.FindProducts(category1.Id, null, null);
                var search2 = productSearchRepository.FindProducts(category2.Id, null, null);
                var search3 = productSearchRepository.FindProducts(category3.Id, null, null);
                var search4 = productSearchRepository.FindProducts(null, null, "Estilo Premier");
                var search5 = productSearchRepository.FindProducts(null, null, "bad search biyotch");

                Assert.IsTrue(search1.Count > 0);
                Assert.IsTrue(search2.Count > 0);
                Assert.IsTrue(search3.Count == 0);
                Assert.IsTrue(search4.Count > 0);
                Assert.IsTrue(search5.Count == 0);

                //Console.WriteLine("\n");
                //Console.WriteLine(JsonConvert.SerializeObject(allProducts, Formatting.Indented));
            }
        }
    }
}
