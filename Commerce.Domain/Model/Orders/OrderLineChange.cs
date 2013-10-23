namespace Commerce.Application.Model.Orders
{
    public class OrderLineChange
    {
        public OrderLine OrderLine { get; set; }
        public int? Quantity { get; set; }

        public bool All
        {
            get { return Quantity == null; }
        }
    }
}
