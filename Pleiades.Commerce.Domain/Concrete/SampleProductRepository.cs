using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
using Pleiades.Commerce.Domain.Abstract;
using Pleiades.Commerce.Domain.Model;
using Pleiades.Commerce.Domain.NHibernate;

namespace Pleiades.Commerce.Domain.Concrete
{
    public class SampleProductRepository : ISampleProductRepository
    {
        public void Add(SampleProduct input)
        {
            var transaction = new Transaction()
                .Add((session) => { session.Save(input); });

            transaction.Execute();
        }

        public void Update(SampleProduct input)
        {
            var transaction = new Transaction()
                .Add((session) => { session.Update(input); });

            transaction.Execute();
        }

        public void Remove(SampleProduct input)
        {
            var transaction = new Transaction()
                .Add(session => { session.Delete(input); });

            transaction.Execute();
        }

        public ICollection<SampleProduct> GetByCategory(string category)
        {
            var session = OpenSessionFactory.Make();
            return session
                    .CreateCriteria(typeof(SampleProduct))
                    .Add(Restrictions.Eq("Category", category))
                    .List<SampleProduct>();
        }

        public SampleProduct GetById(Guid productId)
        {
            var session = OpenSessionFactory.Make();
            return session.Get<SampleProduct>(productId);
        }

        public SampleProduct GetByName(string name)
        {
            var session = OpenSessionFactory.Make();
            return session
                    .CreateCriteria(typeof(SampleProduct))
                    .Add(Restrictions.Eq("Name", name))
                    .UniqueResult<SampleProduct>();
        }
    }
}
