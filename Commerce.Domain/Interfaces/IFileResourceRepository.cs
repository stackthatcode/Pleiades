using System;
using System.Drawing;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Resources;

namespace Commerce.Domain.Interfaces
{
    public interface IFileResourceRepository
    {
        FileResource AddNew(Bitmap bitmap);
        FileResource AddNew(byte[] bytes);
        byte[] RetrieveBytes(Guid externalId);
        void Delete(int Id);
        string PhysicalFilePath(Guid externalId);
    }
}