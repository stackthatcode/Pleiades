﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Commerce.Domain.Model.Lists
{
    public class Category
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string SEO { get; set; }

        public bool Deleted { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateInserted { get; set; }

        public void Touch()
        {
            this.DateUpdated = DateTime.Now;
        }
    }
}