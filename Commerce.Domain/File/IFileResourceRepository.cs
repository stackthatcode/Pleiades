using System;
using System.Drawing;
using Commerce.Application.File.Entities;

namespace Commerce.Application.File
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