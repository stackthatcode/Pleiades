using System;
using System.Drawing;

namespace Commerce.Domain.Interfaces
{
    public interface IImageProcessor
    {
        Bitmap CreateThumbnail(Bitmap original);
        Bitmap CreateLarge(Bitmap original);
    }
}
