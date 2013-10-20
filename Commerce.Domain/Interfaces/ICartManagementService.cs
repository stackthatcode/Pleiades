using Commerce.Application.Model.Shopping;

namespace Commerce.Application.Interfaces
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
