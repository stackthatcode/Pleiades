using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Commerce.Persist.Database;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Resources;

namespace Commerce.Persist.Concrete
{
    public class FileResourceRepository : IFileResourceRepository
    {
        public PushMarketContext Context { get; set; }
        public string ResourceStorage { get; set; }

        public FileResourceRepository(PushMarketContext context)
        {
            this.Context = context;

            // TODO: see if we want to 
            this.ResourceStorage = ConfigurationManager.AppSettings["ResourceStorage"] ?? @"C:\ResourceStorage";
        }

        public FileResource AddNew(Bitmap bitmap)
        {
            var externalId = Guid.NewGuid();            
            var fileResource = new FileResource()
            {
                ExternalId = externalId,
                Name = "(untitled)",
                DateInserted = DateTime.Now,
                DateUpdated = DateTime.Now,
            };

            //var relativePath = RelativeFilePath(externalId);    // DO WE EVEN USE THIS...?
            var fullFilePath = PhysicalFilePath(externalId);
            var directory = StorageDirectory(externalId);
            
            Directory.CreateDirectory(directory);
            bitmap.Save(fullFilePath, ImageFormat.Jpeg);

            this.Context.FileResources.Add(fileResource);
            return fileResource; 
        }

        public FileResource AddNew(byte[] bytes)  // Unnecessary lambda function
        {
            throw new NotImplementedException();
        }

        public byte[] RetrieveBytes(Guid externalId)
        {
            // #1.) Check the File Resource cache
            // #2.) Check the File cache

            var fileResource = this.Context.FileResources.First(x => x.ExternalId == externalId);
            var fullFilePath = PhysicalFilePath(externalId);
            return File.ReadAllBytes(fullFilePath);
        }

        public void Delete(int Id)
        {
            var dataFileResource = this.Context.FileResources.First(x => x.Id == Id);
            dataFileResource.Deleted = true;
        }

        public FileResource Copy(FileResource original)
        {
            var originalFilePath = PhysicalFilePath(original.ExternalId);
            var newExternalId = Guid.NewGuid();

            var fileResource = new FileResource()
            {
                ExternalId = newExternalId,
                Name = "(untitled)",
                DateInserted = DateTime.Now,
                DateUpdated = DateTime.Now,
            };

            //var relativePath = RelativeFilePath(externalId);    // DO WE EVEN USE THIS...?
            var newFilePath = PhysicalFilePath(newExternalId);
            var directory = StorageDirectory(newExternalId);

            Directory.CreateDirectory(directory);
            File.Copy(originalFilePath, newFilePath);

            this.Context.FileResources.Add(fileResource);
            return fileResource; 
        }

        public string RelativeFilePath(Guid externalId)
        {
            return Path.Combine(externalId.ToString(), "image.jpg");
        }

        public string PhysicalFilePath(Guid externalId)
        {
            return Path.Combine(this.ResourceStorage, RelativeFilePath(externalId));
        }

        public string StorageDirectory(Guid externalId)
        {
            return Path.Combine(this.ResourceStorage, externalId.ToString());
        }
    }
}