using System;
using System.Collections.Generic;
using Pleiades.Data;
using Commerce.Domain.Model.Lists;

namespace Commerce.Domain.Interfaces
{
    public interface IJsonCategoryRepository
    {
        List<JsonCategory> RetrieveAllSectionsNoCategories();
        List<JsonCategory> RetrieveAllSectionsWithCategories();

        List<JsonCategory> RetrieveAllCategoriesBySectionId(int sectionCategoryId);
        JsonCategory RetrieveCategoryAndChildrenById(int Id);

        Func<JsonCategory> Insert(JsonCategory category);
        void Update(JsonCategory category);
        void DeleteCategory(int Id);
        void DeleteSection(int Id);
        void SwapParentChild(int parentId, int newParentId);
    }
}