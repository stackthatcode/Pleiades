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
}