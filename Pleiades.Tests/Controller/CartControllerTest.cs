using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Pleiades.Storefront.Domain.Objects.StoreFront;
using Pleiades.Storefront.Tests.Helpers;
using Pleiades.Storefront.WebUI.Areas.StoreFront.Controllers;
using Pleiades.Storefront.WebUI.Areas.StoreFront.Models;
using Pleiades.Tests.Utilities.General;
using Pleiades.Tests.Utilities.Web;

namespace Pleiades.Storefront.Tests.WebUITests
{
    [TestFixture]
    public class CartControllerTest
    {
        [Test]
        public void AddProductToCart()
        {
            var productRepository = MockRepositoryFactory.CreateProductRepositoryVersion1();
            var cartController = new CartController(productRepository, null);
            var cart = new Cart();
            var product = productRepository.Products.ToList()[0];
            cartController.AddToCart(cart, product.ProductID, null);

            cart.CartItems.Count.ShouldEqual(1);
            cart.CartItems[0].Product.ProductID.ShouldEqual(product.ProductID);
        }

        [Test]
        public void AddProductToCartRedirection()
        {
            var productRepository = MockRepositoryFactory.CreateProductRepositoryVersion1();
            var cartController = new CartController(productRepository, null);
            var cart = new Cart();
            var product = productRepository.Products.ToList()[0];

            var result = cartController.AddToCart(cart, product.ProductID, "someReturnUrl");
            result.ShouldBeRedirectionTo(new { action = "Index", returnUrl = "someReturnUrl" });
        }

        [Test]
        public void ViewCartContents()
        {
            var cart = new Cart();
            var result =
                new CartController(MockRepositoryFactory.CreateProductRepositoryVersion1(), null)
                    .Index(cart, "someResultUrl");

            var viewModel = (CartIndexViewModel)result.ViewData.Model;
            viewModel.Cart.ShouldEqual(cart);
            viewModel.ReturnUrl.ShouldEqual("someResultUrl");
        }
    }
}
