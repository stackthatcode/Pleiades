﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Commerce.Persist.Model.Products
{
    [JsonObject]
    public class JsonProductImage
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public string ImageBundleExternalId { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public int? ProductColorId { get; set; }

        [JsonProperty]
        public int Order { get; set; }
    }
}
