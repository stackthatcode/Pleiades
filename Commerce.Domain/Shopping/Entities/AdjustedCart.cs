namespace Commerce.Application.Shopping.Entities
{
    public class AdjustedCart
    {
        public Cart Cart { get; set; }
        public bool InventoryAdjusted { get; set; }
    }
}
