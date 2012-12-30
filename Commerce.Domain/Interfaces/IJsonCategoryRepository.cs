using System;
using System.Collections.Generic;
using Pleiades.Data;
using Commerce.Domain.Model.Lists.Json;

namespace Commerce.Domain.Interfaces
{
    public interface IJsonCategoryRepository
    {
        List<JsonCategory> RetrieveAllSectionCategories();
        List<JsonCategory> RetrieveJsonBySection(int sectionCategoryId);
        JsonCategory RetrieveJsonById(int Id);
        Func<JsonCategory> Insert(JsonCategory category);
        void Update(JsonCategory category);
        void DeleteSoft(int Id);
        void SwapParentChild(int parentId, int newParentId);
    }
}