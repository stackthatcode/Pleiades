using System.Linq;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Shopping;

namespace Commerce.Application.Concrete.Shopping
{
    public class CartManagementService : ICartManagementService
    {
        private readonly ICartIdentificationService _cartIdentificationService;
        private readonly ICartRepository _cartRepository;
        private readonly IInventoryRepository _inventoryRepository;

        public CartManagementService(ICartIdentificationService cartIdentificationService, 
                ICartRepository cartRepository, IInventoryRepository inventoryRepository)
        {            
            _cartIdentificationService = cartIdentificationService;
            _cartRepository = cartRepository;
            _inventoryRepository = inventoryRepository;
        }

        public AdjustedCart Retrieve()
        {
            var cart = IdempotentCartFetcher();
            var inventoryAdjusted = AdjustInventory(cart);
            return new AdjustedCart { Cart = cart, InventoryAdjusted = inventoryAdjusted };
        }

        private Cart IdempotentCartFetcher()
        {
            var identifier = _cartIdentificationService.GetCurrentRequestCardId();
            if (identifier == null)
            {
                var newIdentifier = _cartIdentificationService.ProvisionNewCartId();
                var cart = new Cart
                    {
                        CartIdentifier = newIdentifier,                        
                    };

                _cartRepository.AddCart(cart);
                return cart;
            }
            else
            {
                var cart = _cartRepository.Retrieve(identifier.Value);
                AdjustInventory(cart);
                return cart;
            }
        }

        private bool AdjustInventory(Cart cart)
        {
            bool AdjustmentsMade = false;
            foreach (var item in cart.CartItems.ToList())
            {
                var inventory = _inventoryRepository.RetreiveBySkuCode(item.Sku.SkuCode);
                if (inventory.Available < item.Quantity)
                {
                    item.Quantity = inventory.Available;
                    AdjustmentsMade = true;
                }
                if (inventory.Available == 0)
                {
                    cart.CartItems.Remove(item);
                    AdjustmentsMade = true;
                }
            }
            return AdjustmentsMade;
        }

        public int AddQuantity(string skuCode, int quantity)
        {
            var cart = IdempotentCartFetcher();
            var cartItem = cart.CartItems.FirstOrDefault(x => x.Sku.SkuCode == skuCode);
            var inventory = _inventoryRepository.RetreiveBySkuCode(skuCode);

            if (inventory.Available == 0)
            {
                cart.CartItems.Remove(cartItem);
                return 0;
            }

            if (cartItem == null)
            {
                var correctedQuantity = quantity >= inventory.Available ? inventory.Available : quantity;

                cart.CartItems.Add(
                    new CartItem
                    {
                        Quantity = correctedQuantity,
                        Sku = inventory,
                    });
                return correctedQuantity;
            }
            else
            {
                var correctedQuantity = 
                    quantity + cartItem.Quantity >= inventory.Available ? inventory.Available : quantity + cartItem.Quantity;

                cartItem.Quantity = correctedQuantity;
                return correctedQuantity;
            }
        }

        public int UpdateQuantity(string skuCode, int quantity)
        {
            var cart = IdempotentCartFetcher();
            var cartItem = cart.CartItems.FirstOrDefault(x => x.Sku.SkuCode == skuCode);
            if (cartItem == null)
            {
                return 0;
            }

            var inventory = _inventoryRepository.RetreiveBySkuCode(skuCode);

            if (inventory.Available == 0)
            {
                cart.CartItems.Remove(cartItem);
                return 0;
            }

            var correctedQuantity = quantity + cartItem.Quantity >= inventory.Available ? inventory.Available : quantity;            
            cartItem.Quantity = correctedQuantity;
            return correctedQuantity;
        }

        public void RemoveItem(string skuCode)
        {
            var cart = IdempotentCartFetcher();
            cart.CartItems.RemoveAll(x => x.Sku.SkuCode == skuCode);
        }
    }
}
