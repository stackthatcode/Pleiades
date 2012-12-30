using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Lists.Json;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Commerce.Persist.Products.Json
{
    public static class CategoryExtensions
    {
        public static List<JsonCategory> ToJsonCategoryList(this List<Category> domainCategoryList, int? parentId)
        {
            var output = new List<JsonCategory>();
            foreach (var domainCategory in domainCategoryList.Where(x => x.ParentId == parentId))
            {
                var modelCategory = ToJsonCategory(domainCategoryList, domainCategory.Id);
                output.Add(modelCategory);
            }

            return output;
        }

        public static JsonCategory ToJsonCategory(this List<Category> domainCategoryList, int? Id)
        {
            var domainCategory = domainCategoryList.First(x => x.Id == Id);
            return new JsonCategory
            {
                Id = domainCategory.Id,
                ParentId = domainCategory.ParentId,
                Name = domainCategory.Name,
                SEO = domainCategory.SEO,
                Categories = ToJsonCategoryList(domainCategoryList, domainCategory.Id),
            };
        }

        public static JsonCategory ToJsonCategory(this Category category)
        {
            return new JsonCategory
            {
                Id = category.Id,
                ParentId = category.ParentId,
                Name = category.Name,
                SEO = category.SEO,
            };
        }
    }
}
