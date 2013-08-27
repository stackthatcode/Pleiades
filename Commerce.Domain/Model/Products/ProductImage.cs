﻿using Commerce.Application.Model.Resources;

namespace Commerce.Application.Model.Products
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
