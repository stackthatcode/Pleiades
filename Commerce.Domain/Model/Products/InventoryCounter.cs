using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Domain.Model.Products
{
    /// <summary>
    /// This won't work.  Maybe if we map the SkuCodes...?
    /// </summary>
    public class InventoryCounter
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public string SkuCode { get; set; }

        public int AvailableQuantity { get; set; }
        public int LockedQuantity { get; set; }
    }
}