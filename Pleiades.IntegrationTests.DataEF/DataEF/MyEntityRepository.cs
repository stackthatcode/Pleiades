using Pleiades.App.Data;
using Pleiades.IntegrationTests.DataEF;

namespace Pleiades.DataEF
{
    public class MyEntityRepository : EFGenericRepository<MyEntity>
    {
        public MyEntityRepository(MyContext context)
            : base(context)
        {
        }
    }
}
