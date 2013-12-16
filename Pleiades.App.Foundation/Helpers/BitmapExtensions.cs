using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Pleiades.App.Helpers
{
    public static class BitmapExtensions
    {
        public static byte[] ToByteArray(this Image image, ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }
    }
}
