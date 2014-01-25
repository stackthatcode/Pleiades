namespace Commerce.Application.Email
{
    public enum TemplateIdentifier
    {
        MasterTemplate,

        AdminOrderReceived,
        AdminOrderItemsRefunded,
        AdminOrderItemsShipped,
        AdminSystemError,
        AdminCustomerInquiry,

        CustomerOrderReceived,
        CustomerOrderItemsRefunded,
        CustomerOrderItemsShipped,
    };
}