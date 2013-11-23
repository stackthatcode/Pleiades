namespace Commerce.Application.Email
{
    public enum TemplateIdentifier
    {
        MasterTemplate,
        EmailSignature,

        AdminOrderReceived,
        AdminOrderItemsRefunded,
        AdminOrderItemsShipped,
        AdminSystemError,

        CustomerOrderReceived,
        CustomerOrderItemsRefunded,
        CustomerOrderItemsShipped,
    };
}