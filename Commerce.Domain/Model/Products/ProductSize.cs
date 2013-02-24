using System;
using System.Collections.Generic;
using Commerce.Persist.Model.Lists;
using Commerce.Persist.Model.Resources;

namespace Commerce.Persist.Model.Products
{
    public class ProductSize
    {
        public int Id { get; set; }
        public string Abbr { get; set; }
        public string Name { get; set; }
        public string SkuCode { get; set; }
        public bool IsDeleted { get; set; }
        public int Order { get; set; }
    }
}
