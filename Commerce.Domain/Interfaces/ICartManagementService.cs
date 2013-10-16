using Commerce.Application.Model.Shopping;

namespace Commerce.Application.Interfaces
{
    public interface ICartManagementService
    {
        AdjustedCart Retrieve();
        CartResponseCodes AddQuantity(string skuCode, int quantity);
        CartResponseCodes UpdateQuantity(string skuCode, int quantity);
        void RemoveItem(string skuCode);
    }
}
