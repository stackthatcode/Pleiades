using System;
using System.Collections.Generic;
using Commerce.Domain.Model.Lists;

namespace Commerce.Domain.Model.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BaseSKUNumber { get; set; }

        public Category Category { get; set; }
        public List<Color> Colors { get; set; }
    }
}
