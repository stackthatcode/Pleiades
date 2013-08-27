using System;
using System.Collections.Generic;
using Commerce.Application.Model.Lists;

namespace Commerce.Application.Interfaces
{
    public interface IJsonColorRepository
    {
        List<JsonColor> RetrieveAll();
        JsonColor Retrieve(int id);
        Func<JsonColor> Insert(JsonColor brand);
        void Update(JsonColor brand);
        void DeleteSoft(JsonColor brand);
    }
}
