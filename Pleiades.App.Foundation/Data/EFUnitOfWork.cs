using System;
using System.Data.Entity;

namespace Pleiades.App.Data
{
    public class EfUnitOfWork : IUnitOfWork
    {        
        public DbContext Context { get; set; }
        public Guid Tracer { get; set; }

        public EfUnitOfWork(DbContext context)
        {
            this.Context = context;
            this.Tracer = Guid.NewGuid();
        }

        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }
    }
}
