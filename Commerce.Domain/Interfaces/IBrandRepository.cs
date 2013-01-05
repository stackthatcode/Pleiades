using System;
using System.Collections.Generic;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;

namespace Commerce.Domain.Interfaces
{
    public interface IBrandRepository
    {
        List<JsonBrand> RetrieveAll();
        JsonBrand Retrieve(int id);
        Func<JsonBrand> Insert(JsonBrand brand);
        void Update(JsonBrand brand);
        void DeleteSoft(JsonBrand brand);
    }
}