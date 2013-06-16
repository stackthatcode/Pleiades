﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using Pleiades.Data;
using Pleiades.Web;
using Commerce.Persist.Concrete;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Lists;

namespace Commerce.Web.Areas.Admin.Controllers
{
    public class SectionController : Controller
    {
        IJsonCategoryRepository Repository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }
        PleiadesContext Context { get; set; }

        public SectionController(IJsonCategoryRepository repository, IUnitOfWork unitOfWork, PleiadesContext context)
        {
            this.Repository = repository;
            this.UnitOfWork = unitOfWork;
            this.Context = context;
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
            return new JsonNetResult(new { });
        }
    }
}