using System;
using System.Drawing;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Resources;

namespace Commerce.Persist.Interfaces
{
    public interface IFileResourceRepository
    {
        FileResource AddNew(Bitmap bitmap);
        FileResource AddNew(byte[] bytes);
        byte[] RetrieveBytes(Guid externalId);
        FileResource Copy(FileResource original);
        void Delete(int Id);
        string PhysicalFilePath(Guid externalId);
    }
}