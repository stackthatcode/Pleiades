using Commerce.Application.Model.Resources;

namespace Commerce.Application.Interfaces
{
    public interface IBlankImageRepository
    {
        string BlankImageBySize(ImageSize size);
    }
}
