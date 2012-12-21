using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Pleiades.Data;

namespace Pleiades.Data.EF
{
    public class EFGenericRepository<T> : IGenericRepository<T>
            where T : class
    {
        protected DbContext Context { get; set; }

        public EFGenericRepository(DbContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// All queryable operations should work directly with this
        /// </summary>
        protected virtual IQueryable<T> TrackableData()
        {
            return this.Context.Set<T>();
        }

        protected virtual IQueryable<T> ReadOnlyData()
        {
            return this.Context.Set<T>().AsNoTracking<T>();
        }

        public virtual void Insert(T entity)
        {
            this.Context.Set<T>().Add(entity);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            var query = this.TrackableData().Where(predicate);
            return query;
        }

        public virtual T FindFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return this.TrackableData().FirstOrDefault(predicate);
        }

        public virtual IQueryable<T> GetAll()
        {
            var query = this.TrackableData();
            return query;
        }

        public virtual void Delete(T entity)
        {
            this.Context.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            this.Context.Entry(entity).State = System.Data.EntityState.Modified;
        }

        public virtual int Count()
        {
            return this.GetAll().Count();
        }

        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return this.FindBy(predicate).Count();
        }
    }
}