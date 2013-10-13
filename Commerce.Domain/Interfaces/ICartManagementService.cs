using Commerce.Application.Model.Shopping;

namespace Commerce.Application.Interfaces
{
    public interface ICartManagementService
    {
        AdjustedCart Retrieve();
        int AddQuantity(string skuCode, int quantity);
        int UpdateQuantity(string skuCode, int quantity);
        void RemoveItem(string skuCode);
    }
}
