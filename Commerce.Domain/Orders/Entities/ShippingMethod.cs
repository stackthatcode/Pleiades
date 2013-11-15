namespace Commerce.Application.Orders.Entities
{
    public class ShippingMethod
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
    }
}