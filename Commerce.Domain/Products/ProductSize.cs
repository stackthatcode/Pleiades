namespace Commerce.Application.Products
{
    public class ProductSize
    {
        public int Id { get; set; }
        public string Abbr { get; set; }
        public string Name { get; set; }
        public string SkuCode { get; set; }
        public bool IsDeleted { get; set; }
        public int Order { get; set; }
    }
}
