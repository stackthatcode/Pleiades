﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Domain.Model.Products
{
    /// <summary>
    /// Gucchi, Ikea, Gap, etc.
    /// </summary>
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
