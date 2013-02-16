using System;
using System.Collections.Generic;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Resources;

namespace Commerce.Domain.Model.Products
{
    public class ProductSize
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SEO { get; set; }
        public string SkuCode { get; set; }
        public bool IsDeleted { get; set; }
        public int Order { get; set; }
    }
}