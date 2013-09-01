using System;
using System.Collections.Generic;
using System.Linq;
using Pleiades.Application;
using Pleiades.Application.Data.EF;

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
