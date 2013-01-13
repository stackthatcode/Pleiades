using System;
using System.Collections.Generic;
using Commerce.Domain.Model.Lists;

namespace Commerce.Domain.Interfaces
{
    public interface IColorRepository
    {
        List<JsonColor> RetrieveAll();
        JsonColor Retrieve(int id);
        Func<JsonColor> Insert(JsonColor brand);
        void Update(JsonColor brand);
        void DeleteSoft(JsonColor brand);
    }
}
