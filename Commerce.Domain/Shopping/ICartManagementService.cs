using Commerce.Application.Shopping.Entities;

namespace Commerce.Application.Shopping
{
    public interface ICartManagementService
    {
        AdjustedCart Retrieve();
        AddCartResponseCodes AddQuantity(string skuCode, int quantity);
        AdjustedCart UpdateQuantity(string skuCode, int quantity);
        AdjustedCart UpdateShippingMethod(int shippingMethodId);
        AdjustedCart UpdateStateTax(string stateTaxAbbreviation);
        AdjustedCart RemoveItem(string skuCode);
    }
}
