using System;
using System.Collections.Generic;
using System.Linq;
using Commerce.Domain.Model.Lists;

namespace Commerce.Domain.Model.Products
{
    public class Sku
    {
        public int Id { get; set; }
        public Brand Brand { get; set; }
        public Product Product { get; set; }
        public Color Color { get; set; }
        public Size Size { get; set; }
    }
}
