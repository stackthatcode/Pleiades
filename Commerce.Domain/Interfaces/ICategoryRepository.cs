﻿using System;
using System.Collections.Generic;
using Pleiades.Data;
using Commerce.Domain.Model.Lists;

namespace Commerce.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        List<JsonCategory> RetrieveSectionsOnlyJson();
        List<JsonCategory> RetrieveBySectionIdJson(int sectionCategoryId);
        JsonCategory RetrieveByCategoryIdJson(int Id);
        Func<JsonCategory> Insert(JsonCategory category);
        void Update(JsonCategory category);
        void DeleteCategorySoft(int Id);
        void DeleteSectionSoft(int Id);
        void SwapParentChild(int parentId, int newParentId);
    }
}