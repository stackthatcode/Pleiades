using System;
using System.Collections.Generic;
using Commerce.Domain.Model.Lists;

namespace Commerce.Domain.Model.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BaseSkuCode { get; set; }

        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public SizeGroup SizeGroup { get; set; }
        public List<Size> Sizes { get; set; }
        public List<Color> Colors { get; set; }
    }
}
