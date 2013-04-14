using System;
using System.Collections.Generic;
using Commerce.Persist.Model.Products;

namespace Commerce.Persist.Interfaces
{
    public interface IInventoryRepository
    {
        List<ProductSku> ProductSkuById(int productId);
        int TotalInStock(int productId);
        void Wipe(int productId);
        void UpdateInStock(int productId, int inventoryTotal);
        void Generate(int productId);
        void DeleteByColor(int productId, int productColorId);
        void DeleteBySize(int productId, int productSizeId);
    }
}
