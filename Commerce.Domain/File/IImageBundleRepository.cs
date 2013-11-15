using System;
using System.Drawing;
using Commerce.Application.File.Entities;

namespace Commerce.Application.File
{
    public interface IImageBundleRepository
    {
        ImageBundle AddBitmap(Bitmap original, bool cropThumbnail, bool cropSmall, bool cropLarge);
        ImageBundle AddColor(Color color);
        ImageBundle Retrieve(int Id);
        ImageBundle Retrieve(Guid externalId);
        ImageBundle Copy(int Id);
        void Delete(int id);
    }
}