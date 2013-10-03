using System.Collections.Generic;
using Commerce.Application.Model.Products;

namespace Commerce.Application.Interfaces
{
    public interface IInventoryRepository
    {
        List<ProductSku> RetreiveByProductId(int productId, bool includeChildren);
        int TotalInStock(int productId);
        void Wipe(int productId);
        void UpdateInStock(int productId, int inventoryTotal);
        void UpdateSkuCode(int productId, string newSkuCode);
        void Generate(int productId);
        void DeleteByColor(int productId, int productColorId);
        void DeleteBySize(int productId, int productSizeId);
    }
}
