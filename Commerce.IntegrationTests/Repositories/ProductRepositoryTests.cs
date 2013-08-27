﻿using System;
using System.Linq;
using System.Transactions;
using Autofac;
using NUnit.Framework;
using Pleiades.Application.Data;
using Pleiades.Application.Injection;
using Pleiades.Application;
using Pleiades.Application.EF;
using Pleiades.Application.Utility;
using Commerce.Persist;
using Commerce.Persist.Concrete;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Lists;
using Commerce.Persist.Model.Products;
using Commerce.Initializer.Builders;
using Newtonsoft.Json;

namespace Commerce.IntegrationTests.Repositories
{
    [TestFixture]
    public class ProductRepositoryTests : FixtureBase
    {
        // TODO: how to segment the tests, since this relies on other data being present...?

        [Test]
        public void Empty_And_Repopulate_And_Update_Products()
        {
            using (var lifetime = TestContainer.LifetimeScope())
            {
                var container = lifetime.Resolve<IContainerAdapter>();
                var genericProductRepository = lifetime.Resolve<IGenericRepository<Product>>();
                var categoryRepository = lifetime.Resolve<IGenericRepository<Category>>();
                var productRepository = lifetime.Resolve<IProductRepository>();
                var productSearchRepository = lifetime.Resolve<IProductRepository>();

                BrandBuilder.Populate(container);
                ColorBuilder.Populate(container);
                SizeBuilder.Populate(container);
                CategoryBuilder.Populate(container);
                ProductBuilder.Populate(container);

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
