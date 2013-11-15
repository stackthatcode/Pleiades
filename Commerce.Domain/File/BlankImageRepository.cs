using System.Configuration;
using Commerce.Application.File.Entities;

namespace Commerce.Application.File
{
    public class BlankImageRepository : IBlankImageRepository
    {
        public string BlankImageBySize(ImageSize size)
        {
            if (size == ImageSize.Thumbnail)
            {
                return ConfigurationManager.AppSettings["BlankThumbnailImageUrl"];
            }
            if (size == ImageSize.Small)
            {
                return ConfigurationManager.AppSettings["BlankSmallImageUrl"];
            }   
            else
            {
                return ConfigurationManager.AppSettings["BlankLargeImageUrl"];
            }
        }
    }
}
