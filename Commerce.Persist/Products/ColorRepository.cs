﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Commerce.Persist.Products
{
    public class ColorRepository : EFGenericRepository<Color>, IColorRepository
    {
        public ColorRepository(DbContext context)
            : base(context)
        {
        }

        public List<Color> GetAll()
        {
            throw new NotImplementedException();
        }

        public Color RetrieveWriteable(int Id)
        {
            throw new NotImplementedException();
        }

        public void DeleteSoft(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
