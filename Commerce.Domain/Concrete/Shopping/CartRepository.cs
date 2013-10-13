using System;
using System.Data.Entity;
using System.Linq;
using Commerce.Application.Database;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Shopping;

namespace Commerce.Application.Concrete.Shopping
{
    public class CartRepository : ICartRepository
    {
        private readonly PushMarketContext _context;

        public CartRepository(PushMarketContext context)
        {
            _context = context;
        }

        public void AddCart(Cart cart)
        {
            _context.Carts.Add(cart);
        }

        public Cart Retrieve(Guid identifier)
        {
            return _context.Carts
                .Include(x => x.CartItems)
                .First(x => x.CartIdentifier == identifier);
        }
    }
}
