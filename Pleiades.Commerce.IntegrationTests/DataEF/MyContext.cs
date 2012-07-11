using System.Data.Entity;

namespace Pleiades.Framework.IntegrationTests.DataEF
{
    public class MyContext : DbContext
    {
        public DbSet<MyEntity> MyEntities { get; set; }

        public MyContext() : base("MyTestDatabase")
        {
        }

        public MyContext(string conn) : base(conn)
        {
        }
    }
}
