using System;
using System.Data.Entity;
using System.Transactions;
using Pleiades.Data;

namespace Pleiades.Data.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        Guid _tracer = Guid.NewGuid();

        public Guid Tracer { get { return _tracer; } }

        public DbContext Context { get; set; }

        public EFUnitOfWork(DbContext context)
        {
            this.Context = context;
        }

        public void SaveChanges()
        {
            this.Context.SaveChanges();
        }
    }
}
