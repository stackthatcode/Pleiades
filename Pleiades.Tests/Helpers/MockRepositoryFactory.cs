using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Storefront.Domain.Abstract;
using Pleiades.Storefront.Domain.Objects.StoreFront;
using Rhino.Mocks;

namespace Pleiades.Storefront.Tests.Helpers
{
    public class MockRepositoryFactory
    {
        public static IProductRepository CreateProductRepositoryVersion1()
        {
            MockRepository mocks = new MockRepository();
            IProductRepository repository = mocks.DynamicMock<IProductRepository>();
            SetupResult.For(repository.Products)
                .Return(new List<Product>
                {
                    new Product { ProductID = 1, Category = "Helmets", Name = "Helemt Type 1", Price = 80 },
                    new Product { ProductID = 2, Category = "Helmets", Name = "Helemt Type 2", Price = 90 },
                    new Product { ProductID = 3, Category = "Helmets", Name = "Helemt Elite", Price = 150 },
                    new Product { ProductID = 4, Category = "Leather", Name = "Black Briefcase", Price = 250 },
                    new Product { ProductID = 5, Category = "Leather", Name = "Brown Shoes", Price = 105 },
                }
                .AsQueryable());

            // *** Can Mocks and Stubs be combined...?

            SetupResult.For(repository.Categories)
                .Return(new List<Category>
                {
                    new Category { Name = "Helmets" },
                    new Category { Name = "Leather" },
                }
                .AsQueryable());

            mocks.Replay(repository);
            return repository;
        }

        public static List<string> CreateCategoryList()
        {
            return CreateProductRepositoryVersion1()
                .Products
                .Select<Product, string>(prd => prd.Category)
                .Distinct()
                .ToList();
        }
            
    }
}
