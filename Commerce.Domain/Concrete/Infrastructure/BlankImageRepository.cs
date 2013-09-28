﻿using System.Configuration;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Resources;

namespace Commerce.Application.Concrete.Infrastructure
{
    public class BlankImageRepository : IBlankImageRepository
    {
        public string BlankImageBySize(ImageSize size)
        {
            if (size == ImageSize.Thumbnail)
            {
                return ConfigurationManager.AppSettings["BlankThumbnailImageUrl"];
            }
            else
            {
                return ConfigurationManager.AppSettings["BlankSmallImageUrls"];                
            }
        }
    }
}
