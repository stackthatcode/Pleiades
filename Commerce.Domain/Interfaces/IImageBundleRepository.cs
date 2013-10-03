using System;
using System.Drawing;
using Commerce.Application.Model.Resources;

namespace Commerce.Application.Interfaces
{
    public interface IImageBundleRepository
    {
        ImageBundle AddBitmap(Bitmap original, bool cropThumbnail, bool cropSmall, bool cropLarge);
        ImageBundle AddColor(Color color, int width, int height);
        ImageBundle Retrieve(int Id);
        ImageBundle Retrieve(Guid externalId);
        ImageBundle Copy(int Id);
        void Delete(int id);
    }
}