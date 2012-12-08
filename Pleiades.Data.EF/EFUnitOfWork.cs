using System;
using System.Data.Entity;
using System.Transactions;
using Pleiades.Data;

namespace Pleiades.Data.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; set; }

        public EFUnitOfWork(DbContext context)
        {
            this.Context = context;
        }

        public void Commit()
        {
            this.Context.SaveChanges();
        }
    }
}
