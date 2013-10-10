 using Commerce.Application.Model.Products;

namespace Commerce.Application.Model.Cart
{
    public class CartItem
    {
        public int Id { get; set; }
        public ProductSku Sku { get; set; }
        public int Quantity { get; set; }
    }
}
