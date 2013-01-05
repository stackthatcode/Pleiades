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
    public class ColorRepository : IColorRepository
    {
        PleiadesContext Context { get; set; }

        public ColorRepository(PleiadesContext context)
        {
            this.Context = context;
        }

        protected IQueryable<Color> Data()
        {
            return this.Context.Set<Color>().Where(x => x.Deleted == false);
        }

        public List<Color> RetrieveAll()
        {
            return this.Data().ToList();
        }

        public void Update(Color ColorDiff)
        {
            var Color = this.Data().FirstOrDefault(x => x.Id == ColorDiff.Id);
            Color.Name = ColorDiff.Name;
            Color.SkuCode = ColorDiff.SkuCode;
            Color.DateUpdated = DateTime.Now;
        }

        public Func<Color> Insert(Color ColorDiff)
        {
            var Color = new Color
            {
                Name = ColorDiff.Name,
                SkuCode = ColorDiff.SkuCode,
                DateInserted = DateTime.Now,
                DateUpdated = DateTime.Now,
            };

            this.Context.Set<Color>().Add(Color);
            return () => Color;
        }

        public void DeleteSoft(Color ColorDiff)
        {
            var Color = this.Data().FirstOrDefault(x => x.Id == ColorDiff.Id);
            Color.Deleted = true;
            Color.DateUpdated = DateTime.Now;
        }
    }
}