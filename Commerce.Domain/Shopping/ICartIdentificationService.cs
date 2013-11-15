using System;

namespace Commerce.Application.Shopping
{
    public interface ICartIdentificationService
    {
        Guid ProvisionNewCartId();
        Guid? GetCurrentRequestCardId();
    }
}
