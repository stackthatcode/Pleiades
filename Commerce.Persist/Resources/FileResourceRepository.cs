using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Data.Entity;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Resources;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Commerce.Persist.Resources
{
    public class FileResourceRepository : IFileResourceRepository
    {
        public PleiadesContext Context { get; set; }
        public string ResourceStorage { get; set; }

        public FileResourceRepository(PleiadesContext context)
        {
            this.Context = context;
            this.ResourceStorage = ConfigurationManager.AppSettings["ResourceStorage"] ?? @"C:\ResourceStorage";
        }

        public FileResource AddNew(Bitmap bitmap)
        {
            var externalId = Guid.NewGuid();
            var physicalRelativeStorage = Path.Combine(externalId.ToString(), "file.jpg");

            var fileResource = new FileResource()
            {
                ExternalId = externalId,
                Name = "(untitled)",
                PhysicalRelativeStorage = physicalRelativeStorage,
                DateInserted = DateTime.Now,
                DateUpdated = DateTime.Now,
            };

            bitmap.Save(this.FilePath(physicalRelativeStorage), ImageFormat.Jpeg);
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
            return File.ReadAllBytes(this.FilePath(fileResource.PhysicalRelativeStorage));
        }

        public void Delete(int Id)
        {
            var dataFileResource = this.Context.FileResources.First(x => x.Id == Id);
            dataFileResource.Deleted = true;
        }

        private string FilePath(string relativePath)
        {
            return Path.Combine(this.ResourceStorage, relativePath);
        }
    }
}