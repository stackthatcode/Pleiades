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
    public class SectionController : Controller
    {
        IJsonCategoryRepository Repository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }

        public SectionController(IJsonCategoryRepository repository, IUnitOfWork unitOfWork)
        {
            this.Repository = repository;
            this.UnitOfWork = unitOfWork;
        }

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
        public ActionResult Delete(JsonCategory jsonCategory)
        {
            this.Repository.DeleteSection(jsonCategory.Id.Value);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult();
        }
    }
}