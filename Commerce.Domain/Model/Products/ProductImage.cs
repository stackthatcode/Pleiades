using System;
using System.Collections.Generic;
using Commerce.Persist.Model.Lists;
using Commerce.Persist.Model.Resources;

namespace Commerce.Persist.Model.Products
{
    public class ProductImage
    {
        public int Id { get; set; }
        public ImageBundle ImageBundle { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public ProductColor ProductColor { get; set; }
    }
}
