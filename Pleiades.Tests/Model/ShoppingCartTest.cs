using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Pleiades.Storefront.Domain.Objects.StoreFront;
using Pleiades.Tests.Utilities.Web;
using Pleiades.Storefront.Tests.Helpers;

namespace Pleiades.Storefront.Tests.WebUITests
{
    [TestFixture]
    public class ShoppingCartTest
    {
        [Test]
        public void NewCartEmpty()
        {
            Assert.AreEqual(0, new Cart().CartItems.Count());
        }

        [Test]
        public void CombinesLinesWithSameProduct()
        {
            var cart = new Cart();
            var products = MockRepositoryFactory.CreateProductRepositoryVersion1().Products;
            var product = products.First();

            cart.AddItem(product, 5);
            Assert.AreEqual(5, cart.CartItems.Where(x => x.Product.Equals(product)).First().Quantity);

            cart.AddItem(product, 3);
            Assert.AreEqual(8, cart.CartItems.Where(x => x.Product.Equals(product)).First().Quantity);
        }

        [Test]
        public void ClearCart()
        {
            var cart = new Cart();
            var products = MockRepositoryFactory.CreateProductRepositoryVersion1().Products;
            var product = products.First();

            cart.AddItem(product, 5);
            Assert.AreEqual(1, cart.CartItems.Count());
            
            cart.Clear();
            Assert.AreEqual(0, cart.CartItems.Count());
        }

        [Test]
        public void RemoveProductsTest()
        {
            var cart = new Cart();
            var products = MockRepositoryFactory.CreateProductRepositoryVersion1().Products;
            var product = products.ToList()[0];

            cart.AddItem(product, 5);
            Assert.IsTrue(cart.CartItems.Any(x => x.Product == product));
            cart.RemoveItem(product);
            Assert.IsFalse(cart.CartItems.Any(x => x.Product == product));
        }


        [Test]
        public void TotalValueIsSumOfPrice()
        {
            var cart = new Cart();
            var products = MockRepositoryFactory.CreateProductRepositoryVersion1().Products;
            var product1 = products.ToList()[0];
            var product2 = products.ToList()[1];
            product1.Price = 50;
            product2.Price = 25;

            cart.AddItem(product1, 5);
            cart.AddItem(product2, 2);
            Assert.AreEqual(300m, cart.ComputeTotalValue());
        }
    }
}
