using System;
using System.Data.Entity;
using System.Transactions;
using Pleiades.Application;
using Pleiades.Application.Data;

namespace Pleiades.Application.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {        
        public DbContext Context { get; set; }
        public Guid Tracer { get; set; }

        public EFUnitOfWork(DbContext context)
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
