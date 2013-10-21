using System.Linq;
using Commerce.Application.Database;
using Commerce.Application.Interfaces;
using Commerce.Application.Model.Shopping;

namespace Commerce.Application.Concrete.Shopping
{
    public class CartManagementService : ICartManagementService
    {
        private readonly ICartIdentificationService _cartIdentificationService;
        private readonly ICartRepository _cartRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly PushMarketContext _context;

        public CartManagementService(
                ICartIdentificationService cartIdentificationService, ICartRepository cartRepository, 
                IInventoryRepository inventoryRepository, PushMarketContext context)
        {            
            _cartIdentificationService = cartIdentificationService;
            _cartRepository = cartRepository;
            _inventoryRepository = inventoryRepository;
            _context = context;
        }

        public AdjustedCart Retrieve()
        {
            return IdempotentCartFetcher();
        }

        private AdjustedCart IdempotentCartFetcher()
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
                var inventoryAdjusted = AdjustInventory(cart);
                return new AdjustedCart { Cart = cart, InventoryAdjusted = inventoryAdjusted };
            }
        }

        private AdjustedCart CreateNewCart()
        {
            var newIdentifier = _cartIdentificationService.ProvisionNewCartId();
            var shippingMethod = _context.ShippingMethods.FirstOrDefault();
            var stateTax = _context.StateTaxes.OrderBy(x => x.Abbreviation).FirstOrDefault();
            var cart = new Cart
            {
                CartIdentifier = newIdentifier,
                ShippingMethod = shippingMethod,
                StateTax = stateTax
            };

            _cartRepository.AddCart(cart);
            return new AdjustedCart { Cart = cart, InventoryAdjusted = false };
        }

        private bool AdjustInventory(Cart cart)
        {
            bool adjustmentsMade = false;
            foreach (var item in cart.CartItems.ToList())
            {
                var inventory = item.Sku;
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

        public AddCartResponseCodes AddQuantity(string skuCode, int quantity)
        {
            var cart = IdempotentCartFetcher().Cart;
            var cartItem = cart.CartItems.FirstOrDefault(x => x.Sku.SkuCode == skuCode);
           
            if (cartItem == null)
            {
                var inventory = _inventoryRepository.RetreiveBySkuCode(skuCode);
                
                AddCartResponseCodes responseCode;
                int correctedQuantity;

                if (quantity > inventory.Available)
                {
                    responseCode = AddCartResponseCodes.ReducedQuantityAddedToCart;
                    correctedQuantity = inventory.Available;
                }
                else
                {
                    responseCode = AddCartResponseCodes.FullQuantityAddedToCart;
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
                var inventory = cartItem.Sku;
                if (inventory.Available == 0)
                {
                    cart.CartItems.Remove(cartItem);
                    return AddCartResponseCodes.ItemNoLongerAvailable;
                }

                if (cartItem.Quantity > inventory.Available)
                {
                    cartItem.Quantity = inventory.Available;
                    return AddCartResponseCodes.ReducedQuantityInCart;
                }
                if (cartItem.Quantity == inventory.Available)
                {
                    return AddCartResponseCodes.MaximumQuantityInCart; ;
                }
                if ((cartItem.Quantity + quantity) > inventory.Available)
                {
                    cartItem.Quantity = inventory.Available;
                    return AddCartResponseCodes.ReducedQuantityAddedToCart;
                }
                cartItem.Quantity += quantity;
                return AddCartResponseCodes.FullQuantityAddedToCart;
            }
        }

        public AdjustedCart UpdateQuantity(string skuCode, int quantity)
        {
            var adjustedCart = IdempotentCartFetcher();
            var cart = adjustedCart.Cart;
            var cartItem = cart.CartItems.FirstOrDefault(x => x.Sku.SkuCode == skuCode);

            if (cartItem == null)
            {
                return adjustedCart;
            }

            var inventory = _inventoryRepository.RetreiveBySkuCode(skuCode);

            if (inventory.Available == 0)
            {
                adjustedCart.InventoryAdjusted = true;
                cart.CartItems.Remove(cartItem);
                return adjustedCart;
            }

            if (quantity > inventory.Available)
            {
                adjustedCart.InventoryAdjusted = true;
                cartItem.Quantity = inventory.Available;
                return adjustedCart;
            }
            else
            {
                cartItem.Quantity = quantity;
                return adjustedCart;
            }
        }

        public AdjustedCart UpdateShippingMethod(int shippingMethodId)
        {
            var adjustedCart = IdempotentCartFetcher();
            var method = _context.ShippingMethods.FirstOrDefault(x => x.Id == shippingMethodId);
            adjustedCart.Cart.ShippingMethod = method;
            return adjustedCart;
        }

        public AdjustedCart UpdateStateTax(string stateTaxAbbreviation)
        {
            var adjustedCart = IdempotentCartFetcher();
            var stateTax = _context.StateTaxes.FirstOrDefault(x => x.Abbreviation == stateTaxAbbreviation);
            adjustedCart.Cart.StateTax = stateTax;
            return adjustedCart;            
        }

        public AdjustedCart RemoveItem(string skuCode)
        {
            var adjustedCart = IdempotentCartFetcher();
            adjustedCart.Cart.CartItems.RemoveAll(x => x.Sku.SkuCode == skuCode);
            return adjustedCart;
        }
    }
}
