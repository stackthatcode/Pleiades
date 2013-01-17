using System;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Resources;

namespace Commerce.Domain.Model.Products
{
    public class Image
    {
        public int Id { get; set; }
        public ImageBundle ImageBundle { get; set; }
        public Color Color { get; set; }
    }
}
