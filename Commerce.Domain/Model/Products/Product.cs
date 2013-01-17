using System;
using System.Collections.Generic;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Resources;

namespace Commerce.Domain.Model.Products
{
    public class Product
    {
        // Info Tab
        public int Id { get; set; }
        public string Name { get; set; }
        public string SkuCode { get; set; }
        public string Synopsis { get; set; }
        public string Description { get; set; }
        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }
        public bool Active { get; set; }

        // Colors & Sizes
        public List<Color> Colors { get; set; }
        public Color MainColor { get; set; }
        public SizeGroup SizeGroup { get; set; }

        // Image Tab
        public List<Image> Images { get; set; }
        public Image MainImage { get; set; }

        // back-end
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }
    }
}