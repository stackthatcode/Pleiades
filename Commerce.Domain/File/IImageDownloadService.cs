using System;
using Commerce.Application.File.Entities;

namespace Commerce.Application.File
{
    public interface IImageDownloadService
    {
        ImageDownload Get(Guid imageIdentifier, ImageSize size);
    }
}
