using System;
using System.Drawing;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Resources;

namespace Commerce.Application.Interfaces
{
    public interface IImageBundleRepository
    {
        ImageBundle AddBitmap(Bitmap original);
        ImageBundle AddColor(Color color, int width, int height);
        ImageBundle Retrieve(int Id);
        ImageBundle Retrieve(Guid externalId);
        ImageBundle Copy(int Id);
        void Delete(int id);
    }
}