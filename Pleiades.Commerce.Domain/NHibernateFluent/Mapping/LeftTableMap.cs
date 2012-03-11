using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Pleiades.Commerce.Domain.Model;

namespace Pleiades.Commerce.Domain.NHibernateFluent.Mapping
{
    public class LeftTableMap : ClassMap<LeftTable>
    {
        public LeftTableMap()
        {
            Id(x => x.LeftKey);
            Map(x => x.Data1);
            Map(x => x.Data2);
            Map(x => x.Data3);
        }
    }
}
