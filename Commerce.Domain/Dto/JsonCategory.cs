using System.Collections.Generic;
using System.Linq;
using Commerce.Domain.Model.Lists;
using Newtonsoft.Json;

namespace Commerce.Domain.Dto
{
    [JsonObject]
    public class JsonCategory
    {
        [JsonProperty]
        int? Id { get; set; }

        [JsonProperty]
        int? ParentId { get; set; }

        [JsonProperty]
        List<JsonCategory> Categories { get; set; }

        [JsonProperty]
        string Name { get; set; }

        [JsonProperty]
        string SEO { get; set; }

        public static List<JsonCategory> FromCategoryByParentId(List<Category> domainCategoryList, int parentId)
        {
            var output = new List<JsonCategory>();
            foreach (var domainCategory in domainCategoryList.Where(x => x.ParentId == parentId))
            {
                var modelCategory = FromCategoryById(domainCategoryList, domainCategory.Id);
                output.Add(modelCategory);
            }

            return output;
        }

        public static JsonCategory FromCategoryById(List<Category> domainCategoryList, int Id)
        {
            var domainCategory = domainCategoryList.First(x => x.Id == Id);
            return new JsonCategory
            {
                Id = domainCategory.Id,
                ParentId = domainCategory.ParentId,
                Name = domainCategory.Name,
                SEO = domainCategory.SEO,
                Categories = FromCategoryByParentId(domainCategoryList, domainCategory.Id),
            };
        }
    }
}