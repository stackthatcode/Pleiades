using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using Pleiades.Framework.Data;

namespace Pleiades.Framework.Data.EF
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
        protected virtual IQueryable<T> Data()
        {
            return this.Context.Set<T>();
        }

        public virtual void Add(T entity)
        {
            this.Context.Set<T>().Add(entity);
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            var query = this.Data().Where(predicate);
            return query;
        }

        public virtual T FindFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return this.Data().FirstOrDefault(predicate);
        }

        public virtual IQueryable<T> GetAll()
        {
            var query = this.Data();
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

        public virtual void SaveChanges()
        {
            this.Context.SaveChanges();
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
