using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Pleiades.ConsoleApp.Entities;

namespace Pleiades.ConsoleApp.Mapping
{
    public class StoreMap : ClassMap<Store>
    {
        public StoreMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);

            HasMany(x => x.Staff)
              .Inverse()
              .Cascade.All();

            HasManyToMany(x => x.Products)
             .Cascade.All()
             .Table("StoreProduct");
        }
    }
}
