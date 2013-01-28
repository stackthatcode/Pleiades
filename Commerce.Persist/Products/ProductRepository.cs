using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Products;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Commerce.Persist.Products
{
    public class ProductRepository : IProductRepository
    {
        public List<JsonProductInfo> RetrieveAll()
        {
            throw new NotImplementedException();
        }

        public JsonProductInfo Retrieve(int id)
        {
            throw new NotImplementedException();
        }

        public Func<JsonProductInfo> Insert(JsonProductInfo productInfo)
        {
            throw new NotImplementedException();
        }

        public void Update(JsonProductInfo brand)
        {
            throw new NotImplementedException();
        }

        public void DeleteSoft(JsonProductInfo brand)
        {
            throw new NotImplementedException();
        }
    }
}
