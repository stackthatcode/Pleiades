using Pleiades.Data;
using Commerce.Domain.Model.Lists;
using System.Collections.Generic;

namespace Commerce.Domain.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        List<Category> RetrieveAllSections();
        List<Category> RetrieveAllCategoriesBySection(int sectionCategoryId);
        List<Category> RetrieveByParentId(int Id);
        Category RetrieveById(int Id);
        void Delete(int Id);        
        void SwapParentChild(int parentId, int childId);
    }
}
