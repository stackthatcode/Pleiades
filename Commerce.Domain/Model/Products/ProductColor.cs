using Commerce.Application.Model.Resources;

namespace Commerce.Application.Model.Products
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
