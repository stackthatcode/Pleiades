using Commerce.Application.File.Entities;

namespace Commerce.Application.File
{
    public interface IBlankImageRepository
    {
        string BlankImageBySize(ImageSize size);
    }
}
