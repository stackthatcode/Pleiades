using System.Data.Entity;

namespace Pleiades.IntegrationTests.DataEF
{
    public class MyContext : DbContext
    {
        public DbSet<MyEntity> MyEntities { get; set; }

        public MyContext() : base("PleiadesDb")
        {
        }

        public MyContext(string conn) : base(conn)
        {
        }
    }
}
