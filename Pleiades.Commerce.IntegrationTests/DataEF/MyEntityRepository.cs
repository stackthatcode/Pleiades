using System;
using System.Collections.Generic;
using System.Linq;
using Pleiades.Framework.Data;
using Pleiades.Framework.Data.EF;

namespace Pleiades.Framework.IntegrationTests.DataEF
{
    public class MyEntityRepository : EFGenericRepository<MyEntity>
    {
        public MyEntityRepository(MyContext context)
            : base(context)
        {
        }
    }
}
