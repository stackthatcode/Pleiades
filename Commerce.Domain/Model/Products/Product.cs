﻿using System;
using System.Collections.Generic;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Resources;

namespace Commerce.Domain.Model.Products
{
    public class Product
    {
        public Product()
        {
            Colors = new List<ProductColor>();
            Images = new List<ProductImage>();
            DateCreated = DateTime.Now;
            LastModified = DateTime.Now;
        }

        // Info Tab
        public int Id { get; set; }
        public string Name { get; set; }
        public string SkuCode { get; set; }
        public string SEO { get; set; }
        public string Synopsis { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }        
        public Brand Brand { get; set; }

        // Category and Size Tab
        public Category Category { get; set; }
        public SizeGroup SizeGroup { get; set; }
        public bool Active { get; set; }

        // Colors Tab
        public List<ProductColor> Colors { get; set; }

        // Images Tab
        public List<ProductImage> Images { get; set; }

        // back-end
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }
    }
}