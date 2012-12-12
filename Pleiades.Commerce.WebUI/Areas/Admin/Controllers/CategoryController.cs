using System;
using System.Web;
using System.Web.Mvc;
using Pleiades.Data;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;

namespace Pleiades.Commerce.WebUI.Areas.Admin.Controllers
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

        public JsonResult RetrieveAllSections()
        {
            var result = this.Repository.RetrieveAllSections();
            return Json(result);
        }


        // Should we use this Newtonsoft.Json.dll...? => TODO
    }
}
