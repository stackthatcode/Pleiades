﻿using System.Drawing;
using System.Web.Mvc;
using Commerce.Application.File;
using Commerce.Application.Lists;
using Commerce.Application.Lists.Entities;
using Commerce.Web.Models.Color;
using Pleiades.App.Data;
using Pleiades.Web.Json;

namespace Commerce.Web.Controllers
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
            var imageBundle = this.ImageRepository.AddColor(color);
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
