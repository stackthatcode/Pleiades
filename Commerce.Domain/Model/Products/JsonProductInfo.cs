using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Commerce.Persist.Model.Products
{
    [JsonObject]
    public class JsonProductInfo
    {
        [JsonProperty]
        public int? Id { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Synopsis { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public decimal UnitPrice { get; set; }

        [JsonProperty]
        public decimal UnitCost { get; set; }

        [JsonProperty]
        public string SkuCode { get; set; }

        [JsonProperty]
        public string SEO { get; set; }

        [JsonProperty]
        public int? BrandId { get; set; }

        [JsonProperty]
        public string BrandName { get; set; }

        [JsonProperty]
        public int? CategoryId { get; set; }

        [JsonProperty]
        public string CategoryName { get; set; }

        [JsonProperty]
        public string ImageBundleExternalId { get; set; }

        [JsonProperty]
        public bool AssignImagesToColors { get; set; }
    }
}
