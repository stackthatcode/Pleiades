using System;
using System.Collections.Generic;
using System.Linq;

namespace Pleiades.Framework.Data
{
    /// <summary>
    /// All-purpose paging functions
    /// </summary>
    public static class Paging
    {
        /// <summary>
        /// Returns IEnumerable items by page number, based on items per page
        /// </summary>
        public static IEnumerable<T> Page<T>(this IEnumerable<T> Items, int PageNumber, int ItemsPerPage)
        {
            return Items
                .Skip((PageNumber - 1) * ItemsPerPage)
                .Take(ItemsPerPage);
        }

        /// <summary>
        /// Returns first Page for which Test delegate returns "true"
        /// </summary>
        public static int FindPageNumber<T>(this IEnumerable<T> items, int itemsPerPage, Func<T, bool> test)
        {
            int currentPageNumber = 1;
            var currentItemsOnPage = items.Page(currentPageNumber, itemsPerPage);

            while (currentItemsOnPage.Count() > 0)
            {
                if (currentItemsOnPage.Any(x => test(x)))
                {
                    return currentPageNumber;
                }

                currentPageNumber++;
                currentItemsOnPage = items.Page(currentPageNumber, itemsPerPage);
            }

            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        public static int MaxPageNumber<T>(this IQueryable<T> entities, int itemsPerPage)
        {
            return (entities.Count() - 1) / itemsPerPage + 1;
        }
    }
}
