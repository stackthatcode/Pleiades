using System;

namespace Pleiades.Web.Security.Concrete
{
    public class CacheEntry<T>
    {
        public DateTime LastRefreshed { get; set; }
        public T Item { get; set; }

        public CacheEntry(T item)
        {
            Item = item;
            LastRefreshed = DateTime.UtcNow;
        }
    }
}
