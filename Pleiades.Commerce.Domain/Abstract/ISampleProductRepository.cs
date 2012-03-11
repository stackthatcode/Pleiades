using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Commerce.Domain.Model;

namespace Pleiades.Commerce.Domain.Abstract
{
    public interface ISampleProductRepository
    {
        void Add(SampleProduct SampleProduct);
        void Update(SampleProduct SampleProduct);
        void Remove(SampleProduct SampleProduct);
        SampleProduct GetById(Guid productId);
        SampleProduct GetByName(string name);
        ICollection<SampleProduct> GetByCategory(string category);
    }
}
