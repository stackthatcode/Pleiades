using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using Pleiades.Commerce.Domain.Model;

namespace Pleiades.Commerce.Domain.NHibernate
{
    public class UnitOfWork : IDisposable
    {
        public ISession Session { get; private set; }
        bool shouldCommit = false;

        public UnitOfWork()
        {
            Session = OpenSessionFactory.Make();
            Session.BeginTransaction();
        }

        public void Commit()
        {
            shouldCommit = true;
        }

        public void Dispose()
        {
            if (shouldCommit)
            {
                Session.Transaction.Commit();
            }
            else
            {
                Session.Transaction.Rollback();
            }

            Session.Dispose();
        }
    }
}
