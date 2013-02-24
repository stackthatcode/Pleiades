using System;
using System.Drawing;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Resources;

namespace Commerce.Persist.Interfaces
{
    public interface IImageBundleRepository
    {
        ImageBundle Add(Bitmap original);
        ImageBundle Add(Color color, int width, int height);
        ImageBundle Retrieve(int Id);
        ImageBundle Retrieve(Guid externalId);
        ImageBundle Copy(int Id);
        void Delete(int Id);
    }
}