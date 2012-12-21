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
    public class CategoryController : Controller
    {
        ICategoryRepository Repository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }

        public CategoryController(ICategoryRepository repository, IUnitOfWork unitOfWork)
        {
            this.Repository = repository;
            this.UnitOfWork = unitOfWork;
        }

        // TODO: write a custom Model Binder to map parentId and childId to Id

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Sections()
        {
            var result = this.Repository.RetrieveAllSectionCategories();
            return new JsonNetResult(result);
        }

        [HttpGet]
        public ActionResult CategoriesBySection(int id)
        {
            var result = this.Repository.RetrieveJsonBySection(id);
            return new JsonNetResult(result);
        }

        [HttpGet]
        public ActionResult Category(int id)
        {
            var result = this.Repository.RetrieveJsonById(id);
            return new JsonNetResult(result);
        }

        [HttpPost]
        public ActionResult Insert(JsonCategory jsonCategory)
        {
            var category = 
                new Category()
                {
                    Name = jsonCategory.Name,
                    SEO = jsonCategory.SEO,
                    ParentId = jsonCategory.ParentId,
                };

            this.Repository.Insert(category);
            this.UnitOfWork.SaveChanges();

            jsonCategory.Id = category.Id;
            return new JsonNetResult(jsonCategory);
        }

        [HttpPost]
        public ActionResult Update(JsonCategory jsonCategory)
        {
            var category = this.Repository.RetrieveWriteable(jsonCategory.Id.Value);

            category.ParentId = jsonCategory.ParentId;
            category.Name = jsonCategory.Name;
            category.SEO = jsonCategory.SEO;
            this.UnitOfWork.SaveChanges();

            return new JsonNetResult(jsonCategory);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            this.Repository.DeleteSoft(id);
            this.UnitOfWork.SaveChanges();

            // What do we return...?
            return new JsonNetResult();
        }

        [HttpPost]
        public ActionResult SwapParentChild(int parentId, int childId)
        {
            this.Repository.SwapParentChild(parentId, childId);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult();
        }
    }
}