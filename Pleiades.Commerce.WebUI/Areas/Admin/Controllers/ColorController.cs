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
    public class ColorController : Controller
    {
        IColorRepository Repository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }

        public ColorController(IColorRepository repository, IUnitOfWork unitOfWork)
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
        public ActionResult Colors()
        {
            var result = this.Repository.RetrieveAll();
            return new JsonNetResult(result);
        }

        [HttpPost]
        public ActionResult Insert(Color color)
        {
            var saveResult = this.Repository.Insert(color);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult(saveResult());
        }

        [HttpPost]
        public ActionResult Update(Color color)
        {
            this.Repository.Update(color);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult(color);
        }

        [HttpPost]
        public ActionResult Delete(Color color)
        {
            this.Repository.DeleteSoft(color);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult();
        }
    }
}
