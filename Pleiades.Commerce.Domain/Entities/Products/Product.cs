using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Commerce.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKUNumber { get; set; }

        public Color Color { get; set; }
        public StandardSize Size { get; set; }
    }
}
