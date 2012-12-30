using System.Collections.Generic;
using Pleiades.Data;
using Commerce.Domain.Model.Lists;

namespace Commerce.Domain.Interfaces
{
    public interface ISizeGroupRepository : IGenericRepository<SizeGroup>
    {
        List<JsonSizeGroup> GetAllJson();
        SizeGroup RetrieveWriteable(int Id);
        void DeleteSoft(int Id);
    }
}
