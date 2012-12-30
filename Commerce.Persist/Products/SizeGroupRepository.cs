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
    public class SizeGroupRepository : EFGenericRepository<SizeGroup>, ISizeGroupRepository
    {
        public SizeGroupRepository(DbContext context) : base(context)
        {
        }

        protected override IQueryable<SizeGroup> Data()
        {
            return base.Data().Where(x => x.Deleted == false);
        }

        // COMMENT: isn't this absurd...???  Why use a Repository like this...???

        public List<JsonSizeGroup> GetAllJson()
        {
            var sizes = this.Context
                .ReadOnlyData<Size>()
                .Include(x => x.SizeGroup)
                .ToList();

            var sizeGroups = sizes
                .Select(x => x.SizeGroup.ToJsonSizeGroup())
                .ToList();

            return sizeGroups;
        }

        public SizeGroup RetrieveWriteable(int Id)
        {
            return this.FirstOrDefault(x => x.ID == Id);
        }

        public override void Insert(SizeGroup sizeGroup)
        {
            sizeGroup.DateInserted = DateTime.Now;
            sizeGroup.DateUpdated = DateTime.Now;
            base.Insert(sizeGroup);
        }

        public void DeleteSoft(int Id)
        {
            var sizeGroup = this.RetrieveWriteable(Id);
            sizeGroup.Deleted = true;
        }
    }


    public static class SizeGroupExtensions
    {
        public static JsonSizeGroup ToJsonSizeGroup(this SizeGroup sizeGroup)
        {
            return new JsonSizeGroup
            {
                Id = sizeGroup.ID,
                Name = sizeGroup.Name,
            };
        }
    }
}
