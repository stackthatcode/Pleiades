﻿using System;
using Commerce.Domain.Model.Resources;

namespace Commerce.Domain.Model.Lists
{
    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SkuCode { get; set; }
        public string SEO { get; set; }
        public ImageBundle ImageBundle { get; set; }
        public bool Deleted { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateInserted { get; set; }
    }
}