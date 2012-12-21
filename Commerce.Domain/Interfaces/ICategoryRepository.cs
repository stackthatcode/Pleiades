using Pleiades.Data;
using Commerce.Domain.Model.Lists;
using System.Collections.Generic;

namespace Commerce.Domain.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        List<Category> RetrieveAllSectionCategories(); // TODO: move to ISectionRepository


        List<JsonCategory> RetrieveJsonBySection(int sectionCategoryId);
        JsonCategory RetrieveJsonById(int Id);

        Category RetrieveWriteable(int Id);
        void DeleteSoft(int Id);
        void SwapParentChild(int parentId, int newParentId);
    }
}