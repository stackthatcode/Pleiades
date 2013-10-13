using System.Web.Mvc;
using Commerce.Application.Database;
using Pleiades.Application;
using Pleiades.Application.Data;
using Pleiades.Web;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Lists;
using Pleiades.Web.Json;

namespace Commerce.Web.Areas.Admin.Controllers
{
    public class SectionController : Controller
    {
        IJsonCategoryRepository Repository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }
        PushMarketContext Context { get; set; }

        public SectionController(IJsonCategoryRepository repository, IUnitOfWork unitOfWork, PushMarketContext context)
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