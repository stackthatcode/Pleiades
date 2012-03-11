using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pleiades.Commerce.Domain.Abstract;
using Pleiades.Commerce.Domain.Concrete;
using Pleiades.Commerce.Domain.Model;
using Pleiades.Commerce.WebUI.Areas.Admin.Models;

namespace Pleiades.Commerce.WebUI.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        public const int PageSize = 5;
        public ICategoryService CategoryService { get; set; }

        public CategoryController()
        {
            CategoryService = new CategoryService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Rows(int page = 1, int sortIndex = 1)
        {
            var sortIndexEnum = (CategoryServiceSort)sortIndex;
            var records = CategoryService.RetrievePage(page, CategoryController.PageSize, sortIndexEnum);
            return View("Table", records);
        }

        [HttpPost]
        public JsonResult PageById(int id, int sortIndex = 1)
        {
            var sortIndexEnum = (CategoryServiceSort)sortIndex;
            var pageNumber = CategoryService.RetrievePageNumberById(id, CategoryController.PageSize, sortIndexEnum);
            return Json(new CategoryPageNumber { PageNumber = pageNumber });
        }

        [HttpPost]
        public ViewResult AddRow()
        {
            var blankrecord = new Category()
            {
                Id = -1,
                Active = true,
            };

            return View("EditRow", blankrecord);
        }

        [HttpPost]
        public ViewResult EditRow(int id)
        {
            var record = CategoryService.Retrieve(id);
            return View("EditRow", record);
        }

        [HttpPost]
        public JsonResult SaveRow(Category category)
        {
            category.Name = category.Name ?? "";
            category.Description = category.Description ?? "";

            var id = CategoryService.Save(category);
            var updatedCategory = CategoryService.Retrieve(id);
            return Json(updatedCategory);
        }

        [HttpPost]
        public ActionResult DeleteRow(int id)
        {
            var category = CategoryService.Retrieve(id);
            CategoryService.Delete(category); 
            return View(category);
        }


        // TODO: move this somewhere like a central superclass
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
                return;
            // since we're handling this, log to elmah

            var ex = filterContext.Exception ?? new Exception("No further information exists.");
            //
            //LogException(ex);

            filterContext.ExceptionHandled = true;
            /*
            var data = new ErrorPresentation
            {
                ErrorMessage = HttpUtility.HtmlEncode(ex.Message),
                TheException = ex,
                ShowMessage = !(filterContext.Exception == null),
                ShowLink = false
            };
            filterContext.Result = View("ErrorPage", data);*/
        }

    }
}
