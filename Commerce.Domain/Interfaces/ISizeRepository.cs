using System.Collections.Generic;
using Pleiades.Data;
using Commerce.Domain.Model.Lists;

namespace Commerce.Domain.Interfaces
{
    public interface ISizeRepository : IGenericRepository<Size>
    {
        List<Brand> RetrieveAllByGroup(int groupId);
        Color RetrieveWriteable(int Id);
        void DeleteSoft(int Id);
    }
}
