using System;
using Commerce.Application.Model.Resources;

namespace Commerce.Application.Interfaces
{
    public interface IImageDownloadService
    {
        ImageDownload Get(Guid imageIdentifier, ImageSize size);
    }
}
