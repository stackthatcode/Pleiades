using System;
using System.Linq;
using System.Linq.Expressions;

namespace Pleiades.Data
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        void Insert(T entity);
        void Delete(T entity);
    }
}