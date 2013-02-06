using System;
using System.Collections.Generic;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Resources;

namespace Commerce.Domain.Model.Products
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
