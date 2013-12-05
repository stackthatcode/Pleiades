using System.Web.Mvc;
using Commerce.Application.Lists;
using Commerce.Application.Lists.Entities;
using Pleiades.App.Data;
using Pleiades.Web.Json;

namespace Commerce.Web.Controllers
{
    public class SizeController : Controller
    {
        IJsonSizeRepository Repository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }

        public SizeController(IJsonSizeRepository repository, IUnitOfWork unitOfWork)
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
        public ActionResult SizeGroups()
        {
            var result = this.Repository.RetrieveAll(false);
            return new JsonNetResult(result);
        }

        [HttpGet]
        public ActionResult SizeGroup(int id)
        {
            var result = this.Repository.RetrieveByGroup(id);
            return new JsonNetResult(result);
        }

        [HttpPost]
        public ActionResult InsertGroup(JsonSizeGroup jsonSizeGroup)
        {
            var saveResult = this.Repository.Insert(jsonSizeGroup);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult(saveResult());
        }

        [HttpPost]
        public ActionResult InsertSize(JsonSize jsonSize)
        {
            var saveResult = this.Repository.Insert(jsonSize);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult(saveResult());
        }

        [HttpPost]
        public ActionResult UpdateGroup(JsonSizeGroup jsonSizeGroup)
        {
            this.Repository.Update(jsonSizeGroup);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult(jsonSizeGroup);
        }

        [HttpPost]
        public ActionResult UpdateSize(JsonSize jsonSize)
        {
            this.Repository.Update(jsonSize);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult(jsonSize);
        }


        [HttpPost]
        public ActionResult DeleteGroup(JsonSizeGroup jsonSizeGroup)
        {
            this.Repository.DeleteSoft(jsonSizeGroup);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult();
        }

        [HttpPost]
        public ActionResult DeleteSize(JsonSize jsonSize)
        {
            this.Repository.DeleteSoft(jsonSize);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult();
        }
    }
}