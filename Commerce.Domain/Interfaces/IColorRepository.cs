using System;
using System.Collections.Generic;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;

namespace Commerce.Domain.Interfaces
{
    public interface IColorRepository
    {
        List<Color> RetrieveAll();
        Func<Color> Insert(Color color);
        void Update(Color color);
        void DeleteSoft(Color color);
    }
}
