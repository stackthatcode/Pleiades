using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Commerce.Persist.Products
{
    public class SectionRepository : EFGenericRepository<Category>, ISectionRepository
    {
        public SectionRepository(DbContext context)
            : base(context)
        {
        }


        public List<JsonCategory> GetAll()
        {
            throw new NotImplementedException();
            //return this.Data().
        }

        public Category RetrieveWriteable(int Id)
        {
            throw new NotImplementedException();
        }

        public void DeleteSoft(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
