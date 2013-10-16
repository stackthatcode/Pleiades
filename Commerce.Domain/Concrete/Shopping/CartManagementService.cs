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
                return CreateNewCart();
            }
            else
            {
                var cart = _cartRepository.Retrieve(identifier.Value);
                if (cart == null)
                {
                    return CreateNewCart();
                }
                AdjustInventory(cart);
                return cart;
            }
        }

        private Cart CreateNewCart()
        {
            var newIdentifier = _cartIdentificationService.ProvisionNewCartId();
            var cart = new Cart
            {
                CartIdentifier = newIdentifier,
            };

            _cartRepository.AddCart(cart);
            return cart;            
        }

        private bool AdjustInventory(Cart cart)
        {
            bool adjustmentsMade = false;
            foreach (var item in cart.CartItems.ToList())
            {
                var inventory = _inventoryRepository.RetreiveBySkuCode(item.Sku.SkuCode);
                if (inventory.Available < item.Quantity)
                {
                    item.Quantity = inventory.Available;
                    adjustmentsMade = true;
                }
                if (inventory.Available == 0)
                {
                    cart.CartItems.Remove(item);
                    adjustmentsMade = true;
                }
            }
            return adjustmentsMade;
        }

        public CartResponseCodes AddQuantity(string skuCode, int quantity)
        {
            var cart = IdempotentCartFetcher();
            var cartItem = cart.CartItems.FirstOrDefault(x => x.Sku.SkuCode == skuCode);
            var inventory = _inventoryRepository.RetreiveBySkuCode(skuCode);

            if (inventory.Available == 0)
            {
                cart.CartItems.Remove(cartItem);
                return CartResponseCodes.ItemNoLongerAvailable;
            }

            if (cartItem == null)
            {
                CartResponseCodes responseCode;
                int correctedQuantity;

                if (quantity > inventory.Available)
                {
                    responseCode = CartResponseCodes.ReducedQuantityAddedToCart;
                    correctedQuantity = inventory.Available;
                }
                else
                {
                    responseCode = CartResponseCodes.FullQuantityAddedToCart;
                    correctedQuantity = quantity;
                }

                cartItem = new CartItem()
                    {
                        Quantity = correctedQuantity,
                        Sku = inventory,
                    };
                cart.CartItems.Add(cartItem);
                return responseCode;
            }
            else
            {
                if (cartItem.Quantity >= inventory.Available)
                {
                    cartItem.Quantity = inventory.Available;
                    return CartResponseCodes.ReducedQuantityInCart;
                }
                else
                {
                    if ((cartItem.Quantity + quantity) > inventory.Available)
                    {
                        cartItem.Quantity = inventory.Available;
                        return CartResponseCodes.ReducedQuantityAddedToCart;
                    }
                    else
                    {
                        cartItem.Quantity += quantity;
                        return CartResponseCodes.FullQuantityAddedToCart;
                    }
                }
            }
        }

        public CartResponseCodes UpdateQuantity(string skuCode, int quantity)
        {
            var cart = IdempotentCartFetcher();
            var cartItem = cart.CartItems.FirstOrDefault(x => x.Sku.SkuCode == skuCode);
            if (cartItem == null)
            {
                return CartResponseCodes.ItemNotInCart;
            }

            var inventory = _inventoryRepository.RetreiveBySkuCode(skuCode);

            if (inventory.Available == 0)
            {
                cart.CartItems.Remove(cartItem);
                return CartResponseCodes.ItemNoLongerAvailable;
            }

            if (quantity > inventory.Available)
            {
                cartItem.Quantity = inventory.Available;
                return CartResponseCodes.ReducedQuantityInCart;
            }
            else
            {
                cartItem.Quantity = quantity;
                return CartResponseCodes.FullQuantityUpdatedOnCart;
            }
        }

        public void RemoveItem(string skuCode)
        {
            var cart = IdempotentCartFetcher();
            cart.CartItems.RemoveAll(x => x.Sku.SkuCode == skuCode);
        }
    }
}
