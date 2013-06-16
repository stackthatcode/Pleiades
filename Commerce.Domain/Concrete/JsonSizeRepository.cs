using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Lists;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Commerce.Persist.Concrete
{
    public class JsonSizeRepository : IJsonSizeRepository
    {
        PleiadesContext Context { get; set; }

        public JsonSizeRepository(PleiadesContext context)
        {
            this.Context = context;
        }

        protected IQueryable<Size> SizeData()
        {
            return this.Context.Set<Size>().Where(x => x.Deleted == false);
        }

        protected IQueryable<SizeGroup> SizeGroupData()
        {
            return this.Context.Set<SizeGroup>()
                .Include(x => x.Sizes)
                .Where(x => x.Deleted == false);
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
            this.Context.Sizes.Add(size);
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


        public List<JsonSizeGroup> RetrieveAll(bool includeDefault)
        {
            var query = this
                .SizeGroupData()
                .ToList()
                .Select(x => x.ToJsonSizeGroup());
            if (!includeDefault)
                query = query.Where(x => x.Default == false);

            return query.ToList();
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
                Default = jsonSize.Default,
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