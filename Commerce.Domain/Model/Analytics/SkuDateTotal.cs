using System;
using System.Collections.Generic;

namespace Commerce.Application.Model.Analytics
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
