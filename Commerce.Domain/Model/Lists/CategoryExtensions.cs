﻿using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Domain.Interfaces;
using Pleiades.Data;

namespace Commerce.Domain.Model.Lists
{
    public static class CategoryExtensions
    {       
        public static JsonCategory ToJsonWithChildren(this List<Category> categories, int? Id)
        {
            var category = categories.First(x => x.Id == Id);
            return new JsonCategory
            {
                Id = category.Id,
                ParentId = category.ParentId,
                Name = category.Name,
                SEO = category.SEO,
                Categories = ToJsonListWithChildren(categories, category.Id),
            };
        }

        public static List<JsonCategory> ToJsonListWithChildren(this List<Category> categories, int? parentId)
        {
            var output = new List<JsonCategory>();
            foreach (var domainCategory in categories.Where(x => x.ParentId == parentId))
            {
                var category = ToJsonWithChildren(categories, domainCategory.Id);
                output.Add(category);
            }
            return output;
        }

        public static JsonCategory ToJson(this Category category)
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