using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Commerce.Persist.Lists
{
    public class SizeRepository : ISizeRepository
    {
        DbContext Context { get; set; }

        public SizeRepository(DbContext context)
        {
            this.Context = context;
        }

        protected IQueryable<Size> SizeData()
        {
            return this.Context.Set<Size>().Where(x => x.Deleted == false);
        }

        protected IQueryable<SizeGroup> SizeGroupData()
        {
            return this.Context.Set<SizeGroup>().Include(x => x.Sizes).Where(x => x.Deleted == false);
        }


        public Func<JsonSize> Insert(JsonSize jsonSize)
        {
            var size = new Size
            {
                Name = jsonSize.Name,
                Description = jsonSize.Description,
                SkuCode = jsonSize.SkuCode,
                SEO = jsonSize.SEO,
                DateInserted = DateTime.Now,
                DateUpdated = DateTime.Now,
            };
            var sizeGroup = this.SizeGroupData().First(x => x.ID == jsonSize.ParentGroupId);
            sizeGroup.Sizes.Add(size);
            return () => size.ToJsonSize(sizeGroup.ID);
        }

        public void Update(JsonSize jsonSize)
        {
            var size = this.SizeData().FirstOrDefault(x => x.ID == jsonSize.Id);
            size.Name = jsonSize.Name;
            size.Description = jsonSize.Description;
            size.SkuCode = jsonSize.SkuCode;
            size.SEO = jsonSize.SEO;
            size.DateUpdated = DateTime.Now;
        }

        public void DeleteSoft(JsonSize jsonSize)
        {
            var size = this.SizeData().FirstOrDefault(x => x.ID == jsonSize.Id);
            size.Deleted = true;
            size.DateUpdated = DateTime.Now;
        }


        public List<JsonSizeGroup> RetrieveAll()
        {
            return this
                .SizeGroupData()
                .ToList()
                .Select(x => x.ToJsonSizeGroup())
                .ToList();
        }

        public JsonSizeGroup RetrieveByGroup(int groupId)
        {
            return this
                .SizeGroupData()
                .FirstOrDefault(x => x.ID == groupId)
                .ToJsonSizeGroup();
        }

        public void Update(JsonSizeGroup jsonSizeGroup)
        {
            var sizeGroup = this.SizeGroupData().FirstOrDefault(x => x.ID == jsonSizeGroup.Id);
            sizeGroup.Name = jsonSizeGroup.Name;
            sizeGroup.DateUpdated = DateTime.Now;
        }

        public Func<JsonSizeGroup> Insert(JsonSizeGroup jsonSize)
        {
            var size = new SizeGroup
            {
                Name = jsonSize.Name,
                DateInserted = DateTime.Now,
                DateUpdated = DateTime.Now,
            };

            this.Context.Set<SizeGroup>().Add(size);
            return () => size.ToJsonSizeGroup();
        }

        public void DeleteSoft(JsonSizeGroup jsonSizeGroup)
        {
            var sizeGroup = this.SizeGroupData().FirstOrDefault(x => x.ID == jsonSizeGroup.Id);

            foreach (var size in sizeGroup.Sizes)
            {
                size.Deleted = true;
                //size.SizeGroup = null;
            }

            sizeGroup.Deleted = true;
            sizeGroup.DateUpdated = DateTime.Now;
        }
    }
}