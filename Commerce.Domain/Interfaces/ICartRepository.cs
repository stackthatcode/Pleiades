using System;
using Commerce.Application.Model.Shopping;

namespace Commerce.Application.Interfaces
{
    public interface ICartRepository
    {
        void AddCart(Cart cart);
        Cart Retrieve(Guid identifier);
    }
}
