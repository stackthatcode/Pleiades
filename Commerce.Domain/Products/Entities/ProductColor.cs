using Commerce.Application.File.Entities;

namespace Commerce.Application.Products.Entities
{
    public class ProductColor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SkuCode { get; set; }
        public string SEO { get; set; }
        public virtual ImageBundle ColorImageBundle { get; set; }       // This is a copy of the Color
        public virtual ImageBundle ProductImageBundle { get; set; }     // This is a thumbnail of the Product
        public bool IsDeleted { get; set; }
        public int Order { get; set; }
    }
}
