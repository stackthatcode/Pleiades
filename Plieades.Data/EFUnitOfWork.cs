using System;
using System.Data.Entity;
using System.Transactions;
using Pleiades.Framework.Data;

namespace Pleiades.Framework.Data.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        DbContext Context { get; set; }

        public EFUnitOfWork(DbContext context)
        {
            this.Context = context;
        }

        public void Execute(Action action)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    action.Invoke();
                    transaction.Complete();
                }
                catch
                {
                    // Dispose will trigger a rollback
                    transaction.Dispose();
                }
            }
        }
    }
}
