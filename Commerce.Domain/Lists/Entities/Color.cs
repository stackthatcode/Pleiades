using System;
using System.ComponentModel.DataAnnotations.Schema;
using Commerce.Application.File.Entities;

namespace Commerce.Application.Lists.Entities
{
    public class Color
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SkuCode { get; set; }
        public string SEO { get; set; }

        [ForeignKey("ImageBundleId")]
        public ImageBundle ImageBundle { get; set; }
        public int? ImageBundleId { get; set; }

        public bool Deleted { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateInserted { get; set; }
    }
}