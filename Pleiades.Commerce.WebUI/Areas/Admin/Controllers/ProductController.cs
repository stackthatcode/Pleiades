using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Pleiades.Data;
using Pleiades.Web;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;

namespace Commerce.WebUI.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        IJsonCategoryRepository CategoryRepository { get; set; }
        IJsonBrandRepository BrandRepository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }

        public ProductController(IJsonCategoryRepository categoryRepository, IJsonBrandRepository brandRepository, IUnitOfWork unitOfWork)
        {
            this.CategoryRepository = categoryRepository;
            this.BrandRepository = brandRepository;
            this.UnitOfWork = unitOfWork;
        }


        [HttpGet]   
        public ActionResult Editor()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Categories()
        {
            var result = this.CategoryRepository.RetrieveAllSectionsWithCategories();
            return new JsonNetResult(result);
        }

        [HttpGet]
        public ActionResult Brands()
        {
            var result = this.BrandRepository.RetrieveAll();
            return new JsonNetResult(result);
        }

        [HttpGet]
        public ActionResult Retreive()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult Retreive(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Create()
        {
            throw new NotImplementedException();
        }
    }
}