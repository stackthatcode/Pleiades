using System;
using Commerce.Application.Interfaces;

namespace Commerce.Application.Concrete.Shopping
{
    public class CartIdentificationService : ICartIdentificationService
    {
        public Guid ProvisionNewCartId()
        {
            throw new NotImplementedException();
        }

        public Guid? GetCurrentRequestCardId()
        {
            throw new NotImplementedException();
        }
    }
}
