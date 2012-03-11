using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Xunit;
using Pleiades.Commerce.Domain.Concrete;
using Pleiades.Commerce.Domain.Model;
using Pleiades.Commerce.Domain.NHibernate;


namespace Pleiades.IntegrationTests
{
    public class SampleProductRepositoryTests
    {
        private readonly SampleProduct[] _products = new[]
        {
            new SampleProduct {Name = "Melon", Category = "Fruits"},
            new SampleProduct {Name = "Pear", Category = "Fruits"},
            new SampleProduct {Name = "Milk", Category = "Beverages"},
            new SampleProduct {Name = "Coca Cola", Category = "Beverages"},
            new SampleProduct {Name = "Pepsi Cola", Category = "Beverages"},
        };

        public SampleProductRepositoryTests()
        {
            // Configuration init
            var cfg = new Configuration();
            cfg.Configure().AddAssembly(typeof(SampleProduct).Assembly);

            // Will create the database schema, as needed
            new SchemaExport(cfg).Execute(true, true, false);

            // Populate with Data
            this.Populate();
        }

        public void Populate()
        {
            // Initialize the data
            var actions = new List<Action<ISession>>();
            foreach (var product in _products)
            {
                var productClosure = product;
                actions.Add((session) => { session.Save(productClosure); });
            }

            var transaction = new Transaction(actions);
            transaction.Execute();
        }

        [Xunit.Fact]
        public void Test1_DoesAddWork()
        {
            var repo = new SampleProductRepository();
            var product = new SampleProduct { Category = "Cheeses", Discontinued = true, Name = "Cheddar 3" };
            repo.Add(product);
            repo.Add(product);
            repo.Add(product);

            using (var session = OpenSessionFactory.Make())
            {
                var dbproduct = session.Get<SampleProduct>(product.Id);
                dbproduct.ShouldBeEqualTo(product);
            }
        }

        [Xunit.Fact]
        public void Test2_CanWeUpdateToo()
        {
            var repo = new SampleProductRepository();
            var product = this._products[0];
            product.Name = "Test TESTCHANGE";
            repo.Update(product);

            using (var session = OpenSessionFactory.Make())
            {
                var dbproduct = session.Get<SampleProduct>(product.Id);
                Assert.NotSame(product, dbproduct);
                Assert.Equal("Test TESTCHANGE", dbproduct.Name);
                Assert.Equal(product.Category, dbproduct.Category);
                Assert.Equal(product.Discontinued, dbproduct.Discontinued);
            }
        }

        [Xunit.Fact]
        public void Test3_LetsRemoveARecordFolks()
        {
            var repo = new SampleProductRepository();   
            var product = this._products[0];
            repo.Remove(product);

            using (var session = OpenSessionFactory.Make())
            {
                var dbproduct = session.Get<SampleProduct>(product.Id);
                Assert.Equal(null, dbproduct);
            }
        }

        [Xunit.Fact]
        public void Test4_GetById()
        {
            // Arrange 
            var productId = this._products[0].Id;
            var repo = new SampleProductRepository();

            // Act
            var repositoryProduct = repo.GetById(productId);

            // Assert
            using (var session = OpenSessionFactory.Make())
            {
                var dbproduct = session.Get<SampleProduct>(productId);
                repositoryProduct.ShouldBeEqualTo(dbproduct);
            }
        }

        // NOTE: but shouldn't a repository method that not's retrieving by a Unique Id always return a collection...?

        [Xunit.Fact]
        public void Test5_GetByName()
        {
            // Arrange 
            var name = this._products[0].Name;
            var repo = new SampleProductRepository();

            // Act
            var repositoryProduct = repo.GetByName(name);

            // Assert
            using (var session = OpenSessionFactory.Make())
            {
                var dbproduct = session.Get<SampleProduct>(this._products[0].Id);
                Assert.NotSame(dbproduct, repositoryProduct);
                Assert.Equal(dbproduct.Name, repositoryProduct.Name);
            }
        }

        [Xunit.Fact]
        public void Test6_GetByCategory()
        {
            // Arrange 
            var category = this._products[0].Category;
            var repo = new SampleProductRepository();

            // Act
            var repositoryProductEnumerable = repo.GetByCategory(category);

            // Assert
            using (var session = OpenSessionFactory.Make())
            {
                foreach (var repoProduct in repositoryProductEnumerable)
                {
                    Assert.Equal(category, repoProduct.Category);
                }
            }
        }
    }

    public static class SampleProductTestExtensions
    {
        public static void ShouldBeEqualTo(this SampleProduct actual, SampleProduct expected)
        {
            Assert.NotSame(actual, expected);
            Assert.Equal(actual.Id, expected.Id);
            Assert.Equal(actual.Name, expected.Name);
            Assert.Equal(actual.Category, expected.Category);
            Assert.Equal(actual.Discontinued, expected.Discontinued);
        }
    }
}

