namespace Commerce.Application.Model.Shopping
{
    public enum CartResponseCodes
    {
        FullQuantityAddedToCart,
        FullQuantityUpdatedOnCart,
        ReducedQuantityInCart,
        ReducedQuantityAddedToCart,
        MaximumQuantityInCart,
        ItemNoLongerAvailable,
        ItemNotInCart,
    }
}
