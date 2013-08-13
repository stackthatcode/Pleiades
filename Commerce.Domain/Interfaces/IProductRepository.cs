using System;
using System.Collections.Generic;
using Commerce.Persist.Model.Products;

namespace Commerce.Persist.Interfaces
{
    public interface IProductRepository
    {
        // Info
        List<JsonProductInfo> FindProducts(int? categoryId, int? brandId, string searchText);
        JsonProductInfo RetrieveInfo(int productId);
        void Delete(int productId);
        
        // Colors
        List<JsonProductColor> RetreiveColors(int productId);
        Func<JsonProductColor> AddProductColor(int productId, int colorId);
        void DeleteProductColor(int productId, int colorId);
        void UpdateProductColorSort(int productId, string sortedIds);

        // Sizes
        List<ProductSize> RetrieveSizes(int productId);
        Func<ProductSize> AddProductSize(int productId, int sizeId);
        Func<ProductSize> CreateProductSize(int productId, ProductSize size);
        void DeleteProductSize(int productId, int sizeId);
        void UpdateSizeOrder(int productId, string sortedIds);

        // Images
        List<JsonProductImage> RetrieveImages(int productId);
        Func<JsonProductImage> AddProductImage(int productId, JsonProductImage image);
        void DeleteProductImage(int productId, int imageId);
        void UpdateProductImageSort(int productId, string sortedIds);
        void AssignImagesToColor(int productId);
        void UnassignImagesFromColor(int productId);
        void ChangeImageColor(int productId, int productImageId, int newColor);

    }
}