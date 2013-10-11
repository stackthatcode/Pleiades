using System;
using Commerce.Application.Model.Cart;
using Commerce.Application.Model.Products;

namespace Commerce.Application.Interfaces
{
    public interface ICartRepository
    {
        Cart Retrieve(int id);
        Cart Retrieve(Guid cart_identifier);
        void AddItem(ProductSku sku, int quantity);
        void RemoveItem(ProductSku sku);
        void RemoveItem(int cartItemId);
    }
}
