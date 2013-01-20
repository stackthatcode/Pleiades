using System;
using System.Collections.Generic;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Products;

namespace Commerce.Domain.Interfaces
{
    public interface IProductSearchRepository
    {
        List<Product> FindProducts(int? categoryId, int? brandId, string searchText);
    }
}