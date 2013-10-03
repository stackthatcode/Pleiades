using System;
using System.Configuration;
using System.Drawing;
using Commerce.Application.Interfaces;

namespace Commerce.Application.Concrete.Infrastructure
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
                MaxLargeWidth = Int32.Parse(ConfigurationManager.AppSettings["MaxLargeWidth"]);
            if (ConfigurationManager.AppSettings["MaxLargeHeight"] != null)
                MaxLargeHeight = Int32.Parse(ConfigurationManager.AppSettings["MaxLargeHeight"]);

            if (ConfigurationManager.AppSettings["MaxSmallWidth"] != null)
                MaxSmallWidth = Int32.Parse(ConfigurationManager.AppSettings["MaxSmallWidth"]);
            if (ConfigurationManager.AppSettings["MaxSmallHeight"] != null)
                MaxSmallHeight = Int32.Parse(ConfigurationManager.AppSettings["MaxSmallHeight"]);

            if (ConfigurationManager.AppSettings["MaxThumbnailWidth"] != null)
                MaxThumbnailWidth = Int32.Parse(ConfigurationManager.AppSettings["MaxThumbnailWidth"]);
            if (ConfigurationManager.AppSettings["MaxThumbnailHeight"] != null)
                MaxThumbnailHeight = Int32.Parse(ConfigurationManager.AppSettings["MaxThumbnailHeight"]);
        }

        public Bitmap CreateThumbnail(Bitmap bitmap, bool crop)
        {
            return ResizingProcessor(crop)(bitmap, MaxThumbnailWidth, MaxThumbnailHeight);
        }

        public Bitmap CreateLarge(Bitmap bitmap, bool crop)
        {
            return ResizingProcessor(crop)(bitmap, MaxLargeWidth, MaxLargeHeight);
        }

        public Bitmap CreateSmall(Bitmap bitmap, bool crop)
        {
            return ResizingProcessor(crop)(bitmap, MaxSmallWidth, MaxSmallHeight);
        }


        public Func<Bitmap, int, int, Bitmap> ResizingProcessor(bool crop)
        {
            if (crop)
            {
                return ResizeMinimumDimensionToConstraintsAndCrop;
            }
            else
            {
                return ResizeEntireImageToNewConstraints;
            }
        }

        public Bitmap ResizeEntireImageToNewConstraints(Bitmap bitmap, int targetWidth, int targetHeight)
        {
            if (bitmap.Width <= targetWidth && bitmap.Height <= targetHeight)
            {
                return new Bitmap(bitmap);
            }

            // Determine appropriate aspect ratio and act accordingly
            var targetAspectRatio = AspectRatio(targetWidth, targetHeight);
            var bitmapAspectRatio = AspectRatio(bitmap);

            if (targetAspectRatio < bitmapAspectRatio)
            {
                var factor = (decimal)targetWidth / (decimal)bitmap.Width;
                var newWidth = targetWidth;
                var newHeight = (decimal)bitmap.Height * factor;
                return new Bitmap(bitmap, new Size((int)newWidth, (int)newHeight));
            }
            else
            {
                var factor = (decimal)targetHeight / (decimal)bitmap.Height;
                var newWidth = (decimal)bitmap.Width * factor;
                var newHeight = targetHeight;
                return new Bitmap(bitmap, new Size((int)newWidth, (int)newHeight));
            }
        }

        public Bitmap ResizeMinimumDimensionToConstraintsAndCrop(Bitmap source, int targetWidth, int targetHeight)
        {
            if (source.Width <= targetWidth && source.Height <= targetHeight)
            {
                return new Bitmap(source);
            }

            // e.g. 1:1
            var targetAspectRatio = AspectRatio(targetWidth, targetHeight);
            // e.g. 4:1
            var bitmapAspectRatio = AspectRatio(source);

            if (bitmapAspectRatio > targetAspectRatio)
            {
                var newWidth = (int)(targetWidth * bitmapAspectRatio);
                var newHeight = targetHeight;
                var shrunkImage = new Bitmap(source, newWidth, newHeight);

                // TODO: CROP X-offset and Y-offset
                return (Crop(shrunkImage, targetWidth, targetHeight));
            }
            else
            {
                var newWidth = targetWidth;
                var newHeight = (int)(targetHeight / bitmapAspectRatio);
                var shrunkImage = new Bitmap(source, newWidth, newHeight);

                // TODO: CROP X-offset and Y-offset
                return (Crop(shrunkImage, targetWidth, targetHeight));
            }
        }

        public Bitmap Crop(Bitmap sourceImage, int targetWidth, int targetHeight)
        {
            var cropRect = new Rectangle(0, 0, targetWidth, targetHeight);
            var target = new Bitmap(cropRect.Width, cropRect.Height);

            using(Graphics g = Graphics.FromImage(target))
            {
               g.DrawImage(sourceImage, new Rectangle(0, 0, target.Width, target.Height), 
                                cropRect,                        
                                GraphicsUnit.Pixel);
            }
            return target;
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