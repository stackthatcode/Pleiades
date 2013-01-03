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


        // TODO: create Model for Brands
        
        [HttpGet]
        public ActionResult Brands()
        {
            var result = this.Repository.RetrieveAll();
            return new JsonNetResult(result);
        }


        // TODO: create a data entry Model for Insert

        // STEP #1 - create the Brand record
        [HttpPost]
        public ActionResult Insert(Brand brand)
        {
            var saveResult = this.Repository.Insert(brand);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult(saveResult());
        }

        // STEP #2 - upload the image file
        [HttpPost]
        public ActionResult AddImageBundle(Brand brand)
        {
            throw new NotImplementedException();
        }


        // TODO: create a data entry Model for Update
        [HttpPost]
        public ActionResult Update(Brand brand)
        {
            this.Repository.Update(brand);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult(brand);
        }


        // TODO: create a data entry Model for Delete
        [HttpPost]
        public ActionResult Delete(Brand brand)
        {
            this.Repository.DeleteSoft(brand);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult();
        }
    }
}