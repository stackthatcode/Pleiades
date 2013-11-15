using System;
using Commerce.Application.Shopping.Entities;

namespace Commerce.Application.Shopping
{
    public interface ICartRepository
    {
        void AddCart(Cart cart);
        void AddItemToCart(Cart cart, CartItem cartItem);
        Cart Retrieve(Guid identifier);
    }
}
