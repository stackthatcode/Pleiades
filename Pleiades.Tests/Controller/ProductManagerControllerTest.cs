using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MbUnit.Framework;
using Rhino.Mocks;
using Pleiades.Storefront.Domain.Abstract;
using Pleiades.Storefront.Domain.Objects.StoreFront;
using Pleiades.Storefront.Tests.Helpers;
using Pleiades.Storefront.WebUI.Areas.Admin.Controllers;
using Pleiades.Storefront.WebUI.Areas.StoreFront.Models;
using Pleiades.Tests.Utilities.General;
using Pleiades.Tests.Utilities.Web;

namespace Pleiades.Storefront.Tests.WebUITests
{
    [TestFixture]
    public class ProductManagerControllerTest
    {
        public FormCollection TestFormCollection()
        {
            return new FormCollection
            {
                { "Name", "Some Name" },
                { "Description", "Some Description" },
                { "Price", "1" },
                { "Category", "Some Category" },
            };
        }
        
        [Test]
        public void TestIndexPage()
        {
            var repository = MockRepositoryFactory.CreateProductRepositoryVersion1();
            var controller = new ProductManagerController(repository);
            var result = controller.Index();
            var model = (IEnumerable<Product>)result.Model;
            model.Count().ShouldEqual(repository.Products.Count());
        }

        [Test]
        public void SaveEditedProduct()
        {
            var repository = MockRepository.GenerateMock<IProductRepository>();
            var controller = 
                new ProductManagerController(repository)
                    .WithIncomingValues(TestFormCollection());
            
            var product = new Product() { ProductID = 42 };
            repository.Expect(x => x.ExistingOrNew(42)).Return(product);
            repository.Expect(x => x.SaveProduct(product));

            // Test the POST overload
            var result = controller.Edit(42, null);
            result.ShouldBeRedirectionTo(new { action = "Index" });
            repository.VerifyAllExpectations();
        }

        [Test]
        public void SaveInvalidProduct()
        {
            var repository = MockRepository.GenerateMock<IProductRepository>();
            var controller = 
                new ProductManagerController(repository)
                    .WithIncomingValues(TestFormCollection());
            
            controller.ModelState.AddModelError("oopsie daisy!", "bitch!");
            var product = new Product() { ProductID = 42 };
            repository.Expect(x => x.ExistingOrNew(42)).Return(product);
            repository.Expect(x => x.SaveProduct(product));

            // Test the POST overload
            var result = controller.Edit(42, null);
            result.ShouldBeDefaultView();
        }

        [Test]
        public void DeleteProduct()
        {
            var repository = MockRepository.GenerateMock<IProductRepository>();
            var controller = new ProductManagerController(repository);
            var product = new Product() { ProductID = 24 };
            repository.Expect(x => x.Products).Return(new List<Product>() { product }.AsQueryable());
            repository.Expect(x => x.DeleteProduct(product));
            
            // Test the POST overload
            var result = new ProductManagerController(repository).Delete(product.ProductID);
            result.ShouldBeRedirectionTo(new { action = "Index" });
            repository.VerifyAllExpectations();
        }
    }
}
