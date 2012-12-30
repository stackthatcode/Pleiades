using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Lists.Json;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Commerce.Persist.Products.Json
{
    /// <summary>
    /// Glue to bond stuff between the bounded contexts of EF and Json-grams
    /// </summary>
    public static class SizeExtensions
    {
        public static JsonSize ToJsonSize(this Size size)
        {
            return new JsonSize
            {
                Id = size.ID,
                Name = size.Name,
                SEO = size.SEO,
                SkuCode = size.SkuCode,
                ParentGroupId = size.SizeGroup.ID,
            };
        }

        public static JsonSizeGroup ToJsonSizeGroup(this SizeGroup sizeGroup)
        {
            return new JsonSizeGroup
            {
                Id = sizeGroup.ID,
                Name = sizeGroup.Name,
            };
        }

        public static List<JsonSizeGroup> ToJsonSizeGroup(this List<Size> sizes)
        {
            var sizeGroups = sizes
                .Select(x => x.SizeGroup)
                .Distinct()
                .Select(x => x.ToJsonSizeGroup())
                .ToList();

            foreach (var sizeGroup in sizeGroups)
            {
                foreach (var size in sizes.Where(x => x.SizeGroup.ID == sizeGroup.Id))
                {
                    sizeGroup.Sizes.Add(size.ToJsonSize());
                }
            }

            return sizeGroups;
        }
    }
}