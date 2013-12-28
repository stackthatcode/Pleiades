namespace Commerce.Application.Email
{
    public enum TemplateIdentifier
    {
        MasterTemplate,

        AdminOrderReceived,
        AdminOrderItemsRefunded,
        AdminOrderItemsShipped,
        AdminSystemError,

        CustomerOrderReceived,
        CustomerOrderItemsRefunded,
        CustomerOrderItemsShipped,
    };

    // ALTERNATIVE 
    //public class TemplateIdentifier
    //{
    //    public const int MasterTemplate = 1;

    //    public const int AdminOrderReceived = 2;
    //    public const int AdminOrderItemsRefunded = 3;
    //    public const int AdminOrderItemsShipped = 4;
    //    public const int AdminSystemError = 5;

    //    public const int CustomerOrderReceived = 6;
    //    public const int CustomerOrderItemsRefunded = 7;
    //    public const int CustomerOrderItemsShipped = 8;
    //};
}