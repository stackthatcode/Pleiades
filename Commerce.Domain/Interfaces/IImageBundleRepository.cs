using System;
using System.Drawing;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Resources;

namespace Commerce.Domain.Interfaces
{
    public interface IImageBundleRepository
    {
        ImageBundle Add(Bitmap original);
        ImageBundle Retrieve(int Id);
        ImageBundle Retrieve(Guid externalId);
        void Delete(int Id);
    }
}