using System;
using System.Collections.Generic;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Products;

namespace Commerce.Domain.Interfaces
{
    public interface IProductSearchRepository
    {
        List<JsonProductInfo> FindProducts(int? categoryId, int? brandId, string searchText);
        JsonProductInfo RetrieveInfo(int productId);
        
        List<JsonProductColor> RetreieveColors(int productId);
        JsonProductColor AddProductColor(int productId, int colorId);
        void DeleteProductColor(int productId, int colorId);
        void UpdateProductColorSort(int productId, string sortedIds);

        List<JsonProductImage> RetrieveImages(int productId);
        Func<JsonProductImage> AddProductImage(int productId, JsonProductImage image);
        void DeleteProductImage(int productId, int imageId);
        void UpdateProductImageSort(int productId, string sortedIds);

        void AssignImagesToColor(int productId);
        void UnassignImagesFromColor(int productId);
    }
}