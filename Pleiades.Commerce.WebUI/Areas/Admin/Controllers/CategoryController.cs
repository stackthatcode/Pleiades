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
        IJsonCategoryRepository Repository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }

        public CategoryController(IJsonCategoryRepository repository, IUnitOfWork unitOfWork)
        {
            this.Repository = repository;
            this.UnitOfWork = unitOfWork;
        }

        // TODO: write a custom Model Binder to map parentId and childId to Id

        [HttpGet]
        public ActionResult Editor()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Sections()
        {
            var result = this.Repository.RetrieveAllSectionsNoCategories();
            return new JsonNetResult(result);
        }

        [HttpGet]
        public ActionResult CategoriesBySection(int id)
        {
            var result = this.Repository.RetrieveAllCategoriesBySectionId(id);
            return new JsonNetResult(result);
        }

        [HttpGet]
        public ActionResult Category(int id)
        {
            var result = this.Repository.RetrieveCategoryAndChildrenById(id);
            return new JsonNetResult(result);
        }

        [HttpPost]
        public ActionResult Insert(JsonCategory jsonCategory)
        {
            var saveResult = this.Repository.Insert(jsonCategory);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult(saveResult());
        }

        [HttpPost]
        public ActionResult Update(JsonCategory jsonCategory)
        {
            this.Repository.Update(jsonCategory);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult(jsonCategory);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            this.Repository.DeleteCategory(id);
            this.UnitOfWork.SaveChanges();

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