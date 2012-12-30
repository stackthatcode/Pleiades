using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Pleiades.Data;

namespace Pleiades.Data.EF
{
    /// <summary>
    /// Er, what's the value of this whole thing...? 
    /// 1.) It's an attempt to leverage persistence ignorance
    /// 2.) Defiles the concept of composition over inheritance
    /// 
    /// CONCLUSION: it minially provides a base level of reuse, so keep it for now
    /// </summary>
    public class EFGenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbContext Context { get; set; }

        public EFGenericRepository(DbContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// All queryable operations should work directly with this
        /// </summary>
        protected virtual IQueryable<T> Data()
        {
            return this.Context.Set<T>();
        }

        protected virtual IQueryable<T> ReadOnlyData()
        {
            return this.Data().AsNoTracking<T>();
        }

        public virtual IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return this.Data().Where(predicate);            
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return this.Data().FirstOrDefault(predicate);
        }

        public virtual IQueryable<T> GetAll()
        {
            return this.Data();
        }

        public virtual void Insert(T entity)
        {
            this.Context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            this.Context.Set<T>().Remove(entity);
        }
    }
}