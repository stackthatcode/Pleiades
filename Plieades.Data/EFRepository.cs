using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using Pleiades.Framework.Data;

namespace Pleiades.Framework.Data.EF
{
    public abstract class GenericRepository<T> : IGenericRepository<T>
            where T : class
    {
        protected DbContext Context { get; set; }

        public GenericRepository(DbContext context)
        {
            this.Context = context;
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = this.Context.Set<T>();
            return query;
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = this.Context.Set<T>().Where(predicate);
            return query;
        }

        public T FindFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return this.Context.Set<T>().FirstOrDefault(predicate);
        }

        public virtual void Add(T entity)
        {
            this.Context.Set<T>().Add(entity);
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
