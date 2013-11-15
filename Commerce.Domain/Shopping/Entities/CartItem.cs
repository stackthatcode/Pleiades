 using Commerce.Application.Products;

namespace Commerce.Application.Shopping.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public ProductSku Sku { get; set; }
        public int Quantity { get; set; }
    }
}
