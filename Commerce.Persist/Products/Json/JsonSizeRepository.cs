using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Domain.Model.Lists.Json;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Commerce.Persist.Products.Json
{
    public class JsonSizeRepository : IJsonSizeRepository
    {
        DbContext Context { get; set; }

        public JsonSizeRepository(DbContext context)
        {
            this.Context = context;
        }

        protected IQueryable<Size> SizeData()
        {
            return this.Context.Set<Size>().Where(x => x.Deleted == false);
        }

        protected IQueryable<SizeGroup> SizeGroupData()
        {
            return this.Context.Set<SizeGroup>().Where(x => x.Deleted == false);
        }

        public List<JsonSizeGroup> RetrieveAll()
        {
            var sizes = this
                .SizeData()
                .Include(x => x.SizeGroup)
                .ToList();

            return sizes.ToJsonSizeGroup();
        }

        public List<JsonSizeGroup> RetrieveByGroup(int groupId)
        {
            var sizes = this
                .SizeData()
                .Where(x => x.SizeGroup.ID == groupId)
                .Include(x => x.SizeGroup)
                .ToList();

            return sizes.ToJsonSizeGroup();
        }

        public Func<JsonSize> Insert(JsonSize jsonSize)
        {
            var sizeGroup = this.SizeGroupData().First(x => x.ID == jsonSize.ParentGroupId);

            var size = new Size
            {
                SizeGroup = sizeGroup,
                Name = jsonSize.Name,
                Description = jsonSize.Description,
                SkuCode = jsonSize.SkuCode,
                SEO = jsonSize.SEO,
                DateInserted = DateTime.Now,
                DateUpdated = DateTime.Now,
            };

            this.Context.Set<Size>().Add(size);
            return () => size.ToJsonSize();
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

        public void Update(JsonSize jsonSize)
        {
            var size = this.SizeData().FirstOrDefault(x => x.ID == jsonSize.Id);
            size.Name = jsonSize.Name;
            size.Description = jsonSize.Description;
            size.SkuCode = jsonSize.SkuCode;
            size.SEO = jsonSize.SEO;
            size.DateUpdated = DateTime.Now;
        }

        public void Update(JsonSizeGroup jsonSizeGroup)
        {
            var sizeGroup = this.SizeGroupData().FirstOrDefault(x => x.ID == jsonSizeGroup.Id);
            sizeGroup.Name = jsonSizeGroup.Name;
            sizeGroup.DateUpdated = DateTime.Now;
        }

        public void DeleteSoft(JsonSize jsonSize)
        {
            var size = this.SizeData().FirstOrDefault(x => x.ID == jsonSize.Id);
            size.Deleted = true;
            //size.SizeGroup = null;
            size.DateUpdated = DateTime.Now;
        }

        public void DeleteSoft(JsonSizeGroup jsonSizeGroup)
        {
            var sizeGroup = this.SizeGroupData().FirstOrDefault(x => x.ID == jsonSizeGroup.Id);

            foreach (var size in this.SizeData().Where(x => x.SizeGroup.ID == jsonSizeGroup.Id))
            {
                size.Deleted = true;
                //size.SizeGroup = null;
            }

            sizeGroup.Deleted = true;
            sizeGroup.DateUpdated = DateTime.Now;
        }
    }
}
