using System;
using System.Collections.Generic;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;

namespace Commerce.Domain.Interfaces
{
    public interface IBrandRepository
    {
        List<Brand> RetrieveAll();
        Func<Brand> Insert(Brand brand);
        void Update(Brand brand);
        void DeleteSoft(Brand brand);
    }
}
