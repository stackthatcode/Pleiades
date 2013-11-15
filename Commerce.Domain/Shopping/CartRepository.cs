using System;
using System.Data.Entity;
using System.Linq;
using Commerce.Application.Database;
using Commerce.Application.Shopping.Entities;

namespace Commerce.Application.Shopping
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

        public void AddItemToCart(Cart cart, CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            cart.CartItems.Add(cartItem);
        }

        public Cart Retrieve(Guid identifier)
        {
            var cart = _context.Carts
                .Include(x => x.ShippingMethod)
                .Include(x => x.StateTax)
                .Include(x => x.CartItems)
                .Include(x => x.CartItems.Select(item => item.Sku))
                .FirstOrDefault(x => x.CartIdentifier == identifier);
            if (cart != null)
                _context.RefreshEntity(cart);
            return cart;
        }
    }
}
