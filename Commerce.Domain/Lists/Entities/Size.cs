using System;

namespace Commerce.Application.Lists.Entities
{
    public class Size
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SEO { get; set; }
        public string SkuCode { get; set; }
        public bool Deleted { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateInserted { get; set; }
    }
}
