using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Domain.Model.Products
{
    public class InventoryCounter
    {
        public Product Product { get; set; }
        public int Id { get; set; }
        public int AvailableQuantity { get; set; }
        public int LockedQuantity { get; set; }
    }
}