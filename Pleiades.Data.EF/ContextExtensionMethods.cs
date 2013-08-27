using System.Data.Entity;
using System.Linq;

namespace Pleiades.Application.EF
{
    public static class ContextExtensionMethods
    {
        public static IQueryable<T> ReadOnlyData<T>(this DbContext context) where T : class
        {
            return context.Set<T>().AsNoTracking<T>();
        }

        public static void Insert<T>(this DbContext context, T entity) where T : class
        {
            context.Set<T>().Add(entity);
        }

        public static void Delete<T>(this DbContext context, T entity) where T : class
        {
            context.Set<T>().Remove(entity);
        }
    }
}