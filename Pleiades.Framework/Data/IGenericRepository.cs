using System;
using System.Linq;
using System.Linq.Expressions;

namespace Pleiades.Framework.Data
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T FindFirstOrDefault(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void SaveChanges();
        int Count();
        int Count(Expression<Func<T, bool>> predicate);
    }
}