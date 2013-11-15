using System;
using System.Collections.Generic;
using Commerce.Application.Lists.Entities;

namespace Commerce.Application.Lists
{
    public interface IJsonBrandRepository
    {
        List<JsonBrand> RetrieveAll();
        JsonBrand Retrieve(int id);
        Func<JsonBrand> Insert(JsonBrand brand);
        void Update(JsonBrand brand);
        void DeleteSoft(JsonBrand brand);
    }
}