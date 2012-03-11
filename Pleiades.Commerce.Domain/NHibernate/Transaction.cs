using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;

namespace Pleiades.Commerce.Domain.NHibernate
{
    /// <summary>
    /// This is the Transaction object
    /// </summary>
    public class Transaction
    {
        private readonly List<Action<ISession>> Actions = new List<Action<ISession>>();

        public Transaction()
        {
        }

        public Transaction(List<Action<ISession>> actions)
        {
            this.Actions.AddRange(actions);
        }

        public Transaction Add(Action<ISession> action)
        {
            this.Actions.Add(action);
            return this;
        }

        // MR. OBVIOUS FLAW SAYS ==> how can the individual be enabled to return a strongly typed value...?
        // Er, the more I think about it, this is more a convenience method than anything -- currently!

        public void Execute()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                foreach (var action in this.Actions)
                {
                    action(unitOfWork.Session);
                }

                unitOfWork.Commit();
            }
        }
    }
}
