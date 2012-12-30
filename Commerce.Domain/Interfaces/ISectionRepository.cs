using System;
using System.Collections.Generic;
using Pleiades.Data;
using Commerce.Domain.Model.Lists;

namespace Commerce.Domain.Interfaces
{
    public interface ISectionRepository : IGenericRepository<Category>
    {
        List<JsonCategory> GetAll();
        Category RetrieveWriteable(int Id);
        void DeleteSoft(int Id);
    }
}
