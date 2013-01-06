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
        public int MaxLargeWidth = 800;
        public int MaxLargeHeight = 800;
        public int MaxSmallWidth = 150;
        public int MaxSmallHeight = 150;
        public int MaxThumbnailWidth = 75;
        public int MaxThumbnailHeight = 75;
        public bool AutoCrop = false;

        public ImageProcessor()
        {
            if (ConfigurationManager.AppSettings["MaxLargeWidth"] != null)
            {
                MaxLargeWidth = Int32.Parse(ConfigurationManager.AppSettings["MaxLargeWidth"]);
            }
            if (ConfigurationManager.AppSettings["MaxLargeHeight"] != null)
            {
                MaxLargeHeight = Int32.Parse(ConfigurationManager.AppSettings["MaxLargeHeight"]);
            }
            if (ConfigurationManager.AppSettings["MaxSmallWidth"] != null)
            {
                MaxSmallWidth = Int32.Parse(ConfigurationManager.AppSettings["MaxSmallWidth"]);
            }
            if (ConfigurationManager.AppSettings["MaxSmallHeight"] != null)
            {
                MaxSmallHeight = Int32.Parse(ConfigurationManager.AppSettings["MaxSmallHeight"]);
            }
            if (ConfigurationManager.AppSettings["MaxThumbnailWidth"] != null)
            {
                MaxThumbnailWidth = Int32.Parse(ConfigurationManager.AppSettings["MaxThumbnailWidth"]);
            }
            if (ConfigurationManager.AppSettings["MaxThumbnailHeight"] != null)
            {
                MaxThumbnailHeight = Int32.Parse(ConfigurationManager.AppSettings["MaxThumbnailHeight"]);
            }
        }

        public Bitmap CreateThumbnail(Bitmap bitmap)
        {
            return this.CopyImageToNewConstraints(bitmap, MaxThumbnailWidth, MaxThumbnailHeight);
        }

        public Bitmap CreateLarge(Bitmap bitmap)
        {
            return this.CopyImageToNewConstraints(bitmap, MaxLargeWidth, MaxLargeHeight);
        }

        public Bitmap CreateSmall(Bitmap bitmap)
        {
            return this.CopyImageToNewConstraints(bitmap, MaxSmallWidth, MaxSmallHeight);
        }

        public Bitmap CopyImageToNewConstraints(Bitmap bitmap, int targetWidth, int targetHeight)
        {
            if (bitmap.Width <= targetWidth && bitmap.Height <= targetHeight)
            {
                return new Bitmap(bitmap);
            }

            // Determine appropriate aspect ratio and act accordingly
            var targetAspectRatio = AspectRatio(targetWidth, targetHeight);
            var bitmapAspectRatio = AspectRatio(bitmap);

            if (targetAspectRatio > bitmapAspectRatio)
            {
                var factor = (decimal)targetWidth / (decimal)bitmap.Width;
                var newWidth = targetWidth;
                var newHeight = (decimal)bitmap.Height * factor;
                return new Bitmap(bitmap, new System.Drawing.Size((int)newWidth, (int)newHeight));
            }
            else
            {
                var factor = (decimal)targetHeight / (decimal)bitmap.Height;
                var newWidth = (decimal)bitmap.Width * factor;
                var newHeight = targetHeight;
                return new Bitmap(bitmap, new System.Drawing.Size((int)newWidth, (int)newHeight));
            }

        }

        // Width:Height
        public decimal AspectRatio(Bitmap bitmap)
        {
            return AspectRatio(bitmap.Width, bitmap.Height);
        }

        public decimal AspectRatio(int width, int height)
        {
            return (decimal)width/(decimal)height;
        }
    }
}