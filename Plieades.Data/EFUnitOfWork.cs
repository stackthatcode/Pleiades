using System;
using System.Data.Entity;
using System.Transactions;
using Pleiades.Framework.Data;

namespace Pleiades.Framework.Data.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; set; }

        public EFUnitOfWork(DbContext context)
        {
            this.Context = context;
        }

        public void Execute(Action action)
        {
            bool success = false;

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                action.Invoke();
                scope.Complete();
                success = true;
            }
            
            this.Context.SaveChanges();
        }
    }
}
