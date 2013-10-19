namespace Commerce.Application.Model.Shopping
{
    public enum CartResponseCodes
    {
        FullQuantityAddedToCart = 1,
        FullQuantityUpdatedOnCart = 2,
        ReducedQuantityInCart = 3,
        ReducedQuantityAddedToCart = 4,
        MaximumQuantityInCart = 5,
        ItemNoLongerAvailable = 6,
        ItemNotInCart = 7,
    }
}
