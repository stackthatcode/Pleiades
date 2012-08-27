using System;
using System.Collections.Generic;
using System.Linq;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Pleiades.IntegrationTests.DataEF
{
    public class MyEntityRepository : EFGenericRepository<MyEntity>
    {
        public MyEntityRepository(MyContext context)
            : base(context)
        {
        }
    }
}
