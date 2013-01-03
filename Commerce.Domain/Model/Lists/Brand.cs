﻿using System;
using Newtonsoft.Json;
using Commerce.Domain.Model.Resources;

namespace Commerce.Domain.Model.Lists
{
    [JsonObject]
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SkuCode { get; set; }
        public string SEO { get; set; }
        public ImageBundle ImageBundle { get; set; }
        public bool Deleted { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateInserted { get; set; }
    }
}