using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Domain.Model.Products
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }    // Optional?

        public string SystemResourceId { get; set; }
    }
}
