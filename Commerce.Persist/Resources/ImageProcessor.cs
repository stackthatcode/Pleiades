using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Data.Entity;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Commerce.Persist.Resources
{
    public class ImageProcessor : IImageProcessor 
    {
        public int MaxLargeWidth;
        public int MaxLargeHeight;
        public int MaxThumbnailWidth;
        public int MaxThumbnailHeight;
        public bool AutoCrop;

        public ImageProcessor()
        {
            MaxLargeWidth = Int32.Parse(ConfigurationManager.AppSettings["MaxLargeWidth"] ?? "800");
            MaxLargeHeight = Int32.Parse(ConfigurationManager.AppSettings["MaxLargeHeight"] ?? "800");
            MaxThumbnailWidth = Int32.Parse(ConfigurationManager.AppSettings["MaxThumbnailWidth"] ?? "150");
            MaxThumbnailHeight = Int32.Parse(ConfigurationManager.AppSettings["MaxThumbnailHeight"] ?? "150");
            AutoCrop = Boolean.Parse(ConfigurationManager.AppSettings["AutoCrop"] ?? "false");
        }

        public Bitmap CreateThumbnail(Bitmap bitmap)
        {
            // Determine appropriate aspect ratio and act accordingly

            throw new NotImplementedException();
        }

        public Bitmap CreateLarge(Bitmap bitmap)
        {
            // Determine appropriate aspect ratio and act accordingly

            throw new NotImplementedException();
        }
    }
}