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

using Pleiades.Web.Models;
using Pleiades.Storefront.Domain.Abstract;
using Pleiades.Storefront.Domain.Objects.StoreFront;
using Pleiades.Storefront.Tests.Helpers;
using Pleiades.Storefront.WebUI.Areas.StoreFront.Controllers;
using Pleiades.Storefront.WebUI.Areas.StoreFront.Models;
using Pleiades.Tests.Utilities.General;


namespace Pleiades.Storefront.Tests
{
    [TestFixture]
    public class StoreFrontTests
    {
        [Test]
        public void ViewProductsForAllCategories()
        {
            var controller = new ProductsController(MockRepositoryFactory.CreateProductRepositoryVersion1());
            controller.ItemsPerPage = 5;

            var result = controller.List(null, 1);
            var viewModel = (ProductListModel)result.Model;
            viewModel.CurrentPageItems().Count().ShouldEqual<int>(5);
        }

        [Test]
        public void ViewProductsByCategory()
        {
            var controller = new ProductsController(MockRepositoryFactory.CreateProductRepositoryVersion1());
            controller.ItemsPerPage = 5;

            var result = controller.List("Helmets", 1);
            var viewModel = (ProductListModel)result.Model;
            viewModel.CurrentPageItems().Count().ShouldEqual<int>(3);

            foreach (var item in viewModel.CurrentPageItems())
                item.Category.ShouldEqual("Helmets");
        }

        [Test]
        public void NavMenuHasAlphabeticalListOfDistinctCategories()
        {
            var repository = MockRepositoryFactory.CreateProductRepositoryVersion1();
            var result = new NavigationController(repository).Menu(null);

            // These are produced by the Controller - using the Mock Repository.  Notice, we're removing
            // ... the Home link which has a null category
            var categoryLinks = 
                ((IEnumerable<NavigationLink>)result.ViewData.Model)
                    .Where(x => x.RouteValues["category"] != null);

            // This is from the Repository
            var categories = MockRepositoryFactory.CreateCategoryList();

            // Scan the non-null categories
            Assert.AreElementsEqual(categories, categoryLinks.Select(x => x.Text));

            foreach (var link in categoryLinks)
            {
                link.RouteValues["controller"].ShouldEqual("Products");
                link.RouteValues["action"].ShouldEqual("List");
                link.RouteValues["page"].ShouldEqual("1");
                link.Text.ShouldEqual(link.RouteValues["category"]);
            }
        }

        [Test]
        public void NavMenuShowsHomeLinkAtTop()
        {
            var repository = MockRepositoryFactory.CreateProductRepositoryVersion1();
            var result = new NavigationController(repository).Menu(null);
            var toplink = ((IEnumerable<NavigationLink>)result.ViewData.Model).First();

            toplink.RouteValues["controller"].ShouldEqual("Products");
            toplink.RouteValues["action"].ShouldEqual("List");
            toplink.RouteValues["page"].ShouldEqual("1");
            toplink.RouteValues["category"].ShouldEqual(null);
            toplink.Text.ShouldEqual("Home");
        }
    }
}

