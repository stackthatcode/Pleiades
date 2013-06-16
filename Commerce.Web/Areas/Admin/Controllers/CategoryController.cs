using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pleiades.Data;
using Pleiades.Web;
using Commerce.Persist.Concrete;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Lists;

namespace Commerce.Web.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        IJsonCategoryRepository Repository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }
        PleiadesContext Context { get; set; }

        public CategoryController(IJsonCategoryRepository repository, IUnitOfWork unitOfWork, PleiadesContext context)
        {
            this.Repository = repository;
            this.UnitOfWork = unitOfWork;
            this.Context = context;
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
            return new JsonNetResult(new { Sucess = true });
        }

        [HttpPost]
        public ActionResult SwapParentChild(int parentId, int childId)
        {
            this.Repository.SwapParentChild(parentId, childId);
            this.Context.SaveChanges();
            return new JsonNetResult(new { Sucess = true });
        }
    }
}