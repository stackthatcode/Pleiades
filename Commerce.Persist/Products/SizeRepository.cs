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
    public class SizeRepository : EFGenericRepository<Size>, ISizeRepository
    {
        public SizeRepository(DbContext context)
            : base(context)
        {
        }

        protected override IQueryable<Size> Data()
        {
            return base.Data().Where(x => x.Deleted == false);
        }

        public override void Insert(Size size)
        {
            size.DateInserted = DateTime.Now;
            size.DateUpdated = DateTime.Now;
            base.Insert(size);
        }

        public List<Brand> RetrieveAllByGroup(int groupId)
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


    public static class SizeExtensions
    {
        public static JsonSize ToJsonSize(this Size size)
        {
            return new JsonSize
            {
                Id = size.ID,
                Name = size.Name,
                SEO = size.SEO,
                SkuCode = size.SkuCode,
            };
        }
    }
}