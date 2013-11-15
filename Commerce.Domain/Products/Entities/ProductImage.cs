using Commerce.Application.File.Entities;

namespace Commerce.Application.Products.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public virtual ImageBundle ImageBundle { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public virtual ProductColor ProductColor { get; set; }
    }
}
