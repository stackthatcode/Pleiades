using System;
using System.Collections.Generic;

namespace Commerce.Application.Analytics.Entities
{
    public class SkuDateTotal
    {
        public SkuDateTotal()
        {
            SkuTotals = new List<SkuTotal>();
        }

        public DateTime DateTime { get; set; }
        public List<SkuTotal> SkuTotals { get; set; }
    }
}
