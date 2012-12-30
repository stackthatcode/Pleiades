﻿using System.Collections.Generic;
using Pleiades.Data;
using Commerce.Domain.Model.Lists;

namespace Commerce.Domain.Interfaces
{
    public interface IColorRepository : IGenericRepository<Color>
    {
        List<Color> GetAll();
        Color RetrieveWriteable(int Id);
        void DeleteSoft(int Id);
    }
}
