using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pleiades.Commerce.Domain.Entities.Products
{
    /// <summary>
    /// Represents the aggregate root of a single product.  Every variant (generally color schema) will
    /// be stored in a Product
    /// </summary>
    public class ProductRoot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Brand Brand { get; set; }
        public List<Feature> Features { get; set; }

        // public List<CategoryTag> CategoryTags { get; set; }

        public float SuggestedPrice { get; set; }
        public List<Product> Product { get; set; }
    }
}
