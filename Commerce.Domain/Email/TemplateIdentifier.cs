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
        AdminPasswordReset,

        CustomerOrderReceived,
        CustomerOrderItemsRefunded,
        CustomerOrderItemsShipped,
    };
}