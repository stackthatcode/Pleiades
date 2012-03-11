using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Gallio.Framework;
using MbUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Generated;

using Pleiades.Web.Models;
using Pleiades.Storefront.Domain.Abstract;
using Pleiades.Storefront.Domain.Objects.StoreFront;
using Pleiades.Storefront.Tests.Helpers;
using Pleiades.Storefront.WebUI.Areas.StoreFront.Controllers;
using Pleiades.Storefront.WebUI.Areas.StoreFront.Models;
using Pleiades.Tests.Utilities.General;
using Pleiades.Tests.Utilities.Web;

namespace Pleiades.Storefront.Tests
{
    [TestFixture]
    public class OrderSubmissionTests
    {
        [Test]
        public void CantCheckOutWithEmptyCart()
        {
            var emptyCart = new Cart();
            var shippingDetails = new ShippingDetails();
            var result = new CartController(null, null).CheckOut(emptyCart, shippingDetails);

            // So,how did default view get named String.Empty...?
            result.ShouldBeDefaultView();
        }

        [Test]
        public void CantCheckOutWithINvalidShippingDetails()
        {
            var cart = new Cart();
            var shippingDetails = new ShippingDetails();
            var cartController = new CartController(null, null);
            cartController.ModelState.AddModelError("Name", "That ain't yo name!");

            var result = cartController.CheckOut(cart, new ShippingDetails());
            result.ShouldBeDefaultView();
        }

        [Test]
        public void CanCheckOutAndSubmitOrder()
        {
            var cart = new Cart();
            cart.CartItems.Add(new CartItem { Product = new Product() });

            var shippingDetails = new ShippingDetails();
            var mockOrderSubmitter = MockRepository.GenerateMock<IOrderSubmitter>();
            mockOrderSubmitter.Expect(x => x.SubmitOrder(cart, shippingDetails));

            var cartController = new CartController(null, mockOrderSubmitter);
            var result = cartController.CheckOut(cart, shippingDetails);
            mockOrderSubmitter.VerifyAllExpectations();
        }

        [Test]
        public void AfterCheckingOutCartIsEmptied()
        {
            var cart = new Cart();
            cart.AddItem(new Product(), 1);
            new CartController(null, MockRepository.GenerateStub<IOrderSubmitter>())
                .CheckOut(cart, new ShippingDetails());

            cart.CartItems.Count.ShouldEqual(0);
        }
    }
}

