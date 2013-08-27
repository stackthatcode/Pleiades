using System;
using System.Drawing;

namespace Commerce.Application.Interfaces
{
    public interface IImageProcessor
    {
        Bitmap CreateThumbnail(Bitmap original);
        Bitmap CreateLarge(Bitmap original);
        Bitmap CreateSmall(Bitmap original);
    }
}