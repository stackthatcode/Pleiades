using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Commerce.Domain.Entities
{
    public class InventoryCounter
    {
        public int Id { get; set; }
        public int AvailableQuantity { get; set; }
        public int LockedQuantity { get; set; }
        public InventoryCounter InventoryCounter { get; set; }
    }
}