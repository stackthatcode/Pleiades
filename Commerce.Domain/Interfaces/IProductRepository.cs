using System;
using System.Collections.Generic;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Products;

namespace Commerce.Domain.Interfaces
{
    public interface IProductRepository
    {
        List<JsonProductInfo> RetrieveAll();
        JsonProductInfo Retrieve(int id);
        Func<JsonProductInfo> Insert(JsonProductInfo brand);
        void Update(JsonProductInfo brand);
        void DeleteSoft(JsonProductInfo brand);
    }
}
