using System.Drawing;

namespace Commerce.Application.Interfaces
{
    public interface IImageProcessor
    {
        Bitmap CreateThumbnail(Bitmap original, bool crop);
        Bitmap CreateLarge(Bitmap original, bool crop);
        Bitmap CreateSmall(Bitmap original, bool crop);
    }
}