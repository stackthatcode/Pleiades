using System;
using System.Collections.Generic;
using Commerce.Persist.Model.Lists;
using Commerce.Persist.Model.Resources;

namespace Commerce.Persist.Model.Products
{
    public class ProductColor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SkuCode { get; set; }
        public string SEO { get; set; }
        public ImageBundle ColorImageBundle { get; set; }       // This is a copy of the Color
        public ImageBundle ProductImageBundle { get; set; }     // This is a thumbnail of the Product
        public bool IsDeleted { get; set; }
        public int Order { get; set; }
    }
}
