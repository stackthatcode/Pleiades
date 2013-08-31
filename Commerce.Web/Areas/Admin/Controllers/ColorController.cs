using System.Drawing;
using System.Web.Mvc;
using Pleiades.Application.Data;
using Pleiades.Web;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Lists;
using Commerce.Web.Areas.Admin.Models.Color;

namespace Commerce.Web.Areas.Admin.Controllers
{
    public class ColorController : Controller
    {
        IJsonColorRepository ColorRepository { get; set; }
        IImageBundleRepository ImageRepository { get; set; }
        IUnitOfWork UnitOfWork { get; set; }

        public ColorController(IJsonColorRepository colorRepository, 
                IImageBundleRepository imageRepository, IUnitOfWork unitOfWork)
        {
            this.ColorRepository = colorRepository;
            this.ImageRepository = imageRepository;
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
            var result = this.ColorRepository.RetrieveAll();
            return new JsonNetResult(result);
        }

        [HttpGet]
        public ActionResult Color(int Id)
        {
            var result = this.ColorRepository.Retrieve(Id);
            return new JsonNetResult(result);
        }

        [HttpPost]
        public ActionResult CreateBitmap(CreateColorBitmap request)
        {
            string rgb = request.Rgb;
            var color = ColorTranslator.FromHtml(rgb);
            var imageBundle = this.ImageRepository.Add(color, 150, 150);
            UnitOfWork.SaveChanges();
            return new JsonNetResult(imageBundle);
        }

        [HttpPost]
        public ActionResult Insert(JsonColor color)
        {
            var saveResult = this.ColorRepository.Insert(color);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult(saveResult());
        }

        [HttpPost]
        public ActionResult Update(JsonColor color)
        {
            this.ColorRepository.Update(color);
            this.UnitOfWork.SaveChanges();
            return new JsonNetResult(color);
        }

        [HttpPost]
        public ActionResult Delete(JsonColor color)
        {
            this.ColorRepository.DeleteSoft(color);
            this.UnitOfWork.SaveChanges();
            return JsonNetResult.Success();
        }
    }
}
