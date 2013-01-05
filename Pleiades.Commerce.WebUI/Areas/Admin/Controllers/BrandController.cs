﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pleiades.Data;
using Pleiades.Web;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;

namespace Commerce.WebUI.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        IBrandRepository Repository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }

        public BrandController(IBrandRepository repository, IUnitOfWork unitOfWork)
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
        public ActionResult Brands()
        {
            var result = this.Repository.RetrieveAll();
            return new JsonNetResult(result);
        }

        [HttpPost]
        public ActionResult Insert(JsonBrand brand)
        {
            var saveResult = this.Repository.Insert(brand);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult(saveResult());
        }

        [HttpPost]
        public ActionResult AddImageBundle(Brand brand)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Update(JsonBrand brand)
        {
            this.Repository.Update(brand);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult(brand);
        }

        [HttpPost]
        public ActionResult Delete(JsonBrand brand)
        {
            this.Repository.DeleteSoft(brand);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult();
        }
    }
}