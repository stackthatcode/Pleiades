﻿using System;
using System.Collections.Generic;

namespace Commerce.Domain.Model.Lists
{
    public class SizeGroup
    {
        public SizeGroup()
        {
            this.Sizes = new List<Size>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public List<Size> Sizes { get; set; }
        public bool Deleted { get; set; }        
        public DateTime DateUpdated { get; set; }
        public DateTime DateInserted { get; set; }

        public void Touch()
        {
            this.DateUpdated = DateTime.Now;
        }
    }
}