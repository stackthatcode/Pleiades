using System;
using Commerce.Application.Model.Shopping;

namespace Commerce.Application.Interfaces
{
    public interface ICartRepository
    {
        void AddCart(Cart cart);
        void AddItemToCart(Cart cart, CartItem cartItem);
        Cart Retrieve(Guid identifier);
    }
}
