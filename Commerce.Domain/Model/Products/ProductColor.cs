using System;
using System.Collections.Generic;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Resources;

namespace Commerce.Domain.Model.Products
{
    public class ProductColor
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public Color Color { get; set; }
        public bool IsDeleted { get; set; }
        public int Order { get; set; }
    }
}