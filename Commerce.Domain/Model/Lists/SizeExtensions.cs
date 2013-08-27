﻿using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Application.Interfaces;
using Pleiades.Application;

namespace Commerce.Application.Model.Lists
{
    /// <summary>
    /// Glue to bond stuff between the bounded contexts of EF and Json-grams
    /// </summary>
    public static class SizeExtensions
    {
        public static JsonSize ToJsonSize(this Size size, int parentId)
        {
            return new JsonSize
            {
                Id = size.ID,
                ParentGroupId = parentId,
                Name = size.Name,
                Description = size.Description,
                SEO = size.SEO,
                SkuCode = size.SkuCode,
            };
        }

        public static JsonSizeGroup ToJsonSizeGroup(this SizeGroup sizeGroup)
        {
            return new JsonSizeGroup
            {
                Id = sizeGroup.ID,
                Name = sizeGroup.Name,
                Default = sizeGroup.Default,
                Sizes = sizeGroup.Sizes
                    .Where(x => x.Deleted == false)
                    .Select(x => x.ToJsonSize(sizeGroup.ID)).ToList(),
            };
        }
    }
}