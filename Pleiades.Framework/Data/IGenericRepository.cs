using System;
using System.Linq;
using System.Linq.Expressions;

namespace Pleiades.Data
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T FindFirstOrDefault(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        int Count();
        int Count(Expression<Func<T, bool>> predicate);
    }
}