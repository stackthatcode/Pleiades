using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PagedList;

namespace Pleiades.Utilities.General
{
    /// <summary>
    /// All-purpose paging functions
    /// </summary>
    public static class Paging
    {
        public static IEnumerable<T> Page<T>(this IEnumerable<T> Items, int PageNumber, int ItemsPerPage)
        {
            return Items
                .Skip((PageNumber - 1) * ItemsPerPage)
                .Take(ItemsPerPage);
        }

        public static int FindPageNumber<T>(this IEnumerable<T> items, int itemsPerPage, Func<T, bool> test)
        {
            int currentPageNumber = 1;
            var currentItemsOnPage = items.Page(currentPageNumber, itemsPerPage);

            while (currentItemsOnPage.Count() > 0)
            {
                if (currentItemsOnPage.Any(x => test(x)))
                    return currentPageNumber;

                currentPageNumber++;
                currentItemsOnPage = items.Page(currentPageNumber, itemsPerPage);
            }

            return -1;
        }

        public static IPagedList<T2> PageFromEntities<T1, T2>(this IQueryable<T1> entities, int pageNumber, int itemsPerPage)
                where T2: new()
        {
            var maxPageNumber = entities.MaxPageNumber(itemsPerPage);
            if (pageNumber > maxPageNumber)
                pageNumber = maxPageNumber;
            
            var pagedListFromDb = entities.Page<T1>(pageNumber, itemsPerPage).ToList();
            var pagedListOfModel = pagedListFromDb.Select(x => x.AutoMap<T1, T2>());

            var totalItemCount = entities.Count();

            return new StaticPagedList<T2>(pagedListOfModel, pageNumber - 1, itemsPerPage, totalItemCount);
        }

        public static int MaxPageNumber<T>(this IQueryable<T> entities, int itemsPerPage)
        {
            return (entities.Count() - 1) / itemsPerPage + 1;
        }
    }
}
