using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Commerce.Application.File;
using Commerce.Application.File.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Pleiades.App.Helpers;

namespace Commerce.Application.Azure.File
{
    public class AzureFileResourceRepository : IFileResourceRepository
    {
        private readonly CloudBlobContainer _container;
        private const string RelativeFilePath = "** AZURE STORAGE **";

        public AzureFileResourceRepository(string storageConnectionString, string storageContainerName)
        {
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(storageContainerName);
        }

        public FileResource AddNew(Bitmap bitmap)
        {   
            var bytes = bitmap.ToByteArray(ImageFormat.Png);
            return AddNew(bytes);
        }

        public FileResource AddNew(byte[] bytes)
        {
            var identifier = Guid.NewGuid();
            var blob = _container.GetBlockBlobReference(identifier.ToString());
            blob.UploadFromByteArray(bytes, 0, bytes.Length);

            return new FileResource()
            {
                DateInserted = DateTime.Now,
                DateUpdated = DateTime.Now,
                Deleted = false,
                ExternalId = identifier,
                Name = "",
                RelativeFilePath = RelativeFilePath
            };
        }

        public byte[] RetrieveBytes(Guid externalId)
        {
            var blob = _container.GetBlockBlobReference(externalId.ToString());
            var memoryStream = new MemoryStream();
            blob.DownloadToStream(memoryStream);
            return memoryStream.ToArray();
        }

        public FileResource Copy(FileResource original)
        {
            var blob = _container.GetBlockBlobReference(original.ExternalId.ToString());
            var newBlobIdentifier = Guid.NewGuid();

            var newBlob = _container.GetBlockBlobReference(newBlobIdentifier.ToString());
            newBlob.StartCopyFromBlob(blob);

            return new FileResource()
            {
                DateInserted = DateTime.Now,
                DateUpdated = DateTime.Now,
                Deleted = false,
                ExternalId = newBlobIdentifier,
                Name = original.Name,
                RelativeFilePath = RelativeFilePath
            };
        }

        public void Delete(Guid externalId)
        {
            var blob = _container.GetBlockBlobReference(externalId.ToString());
            blob.DeleteIfExists();
        }

        // TODO: PHASE 2 - actually create a physical cache of the same file
        public string PhysicalFilePath(Guid externalId)
        {
            throw new NotImplementedException();
        }
    }
}
