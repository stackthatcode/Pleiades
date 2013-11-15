using System;
using System.Collections.Generic;

namespace Commerce.Application.File.Entities
{
    public static class ImageSizeExtensions
    {
        private static readonly Dictionary<string, ImageSize> Storage = new Dictionary<string, ImageSize>()
            {
                {"THUMBNAIL", ImageSize.Thumbnail},
                {"ORIGINAL", ImageSize.Original},
                {"LARGE", ImageSize.Large},
                {"SMALL", ImageSize.Small},
            };

        private readonly static
            Dictionary<ImageSize, Func<ImageBundle, FileResource>> Map =
                new Dictionary<ImageSize, Func<ImageBundle, FileResource>>
                        {
                            {ImageSize.Thumbnail, x => x.Thumbnail},
                            {ImageSize.Large, x => x.Large},
                            {ImageSize.Small, x => x.Small},
                            {ImageSize.Original, x => x.Original},
                        };


        public static ImageSize ToImageSize(this string input)
        {
            return Storage.ContainsKey(input.ToUpper()) ? Storage[input.ToUpper()] : ImageSize.Small;
        }

        public static FileResource FileByImageSize(this ImageBundle imageBundle, ImageSize size)
        {
            return Map[size](imageBundle);
        }
    }
}
