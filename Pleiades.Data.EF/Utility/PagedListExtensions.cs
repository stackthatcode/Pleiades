using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Data;
using PagedList;

namespace Pleiades.Data.EF.Utility
{
    public static class PagedListExtensions
    {
        public static IPagedList<T2> PageFromEntities<T1, T2>(this IQueryable<T1> entities, int pageNumber, int itemsPerPage)
                where T2 : new()
        {
            var maxPageNumber = entities.MaxPageNumber(itemsPerPage);
            if (pageNumber > maxPageNumber)
                pageNumber = maxPageNumber;

            var pagedListFromDb = entities.Page<T1>(pageNumber, itemsPerPage).ToList();
            var pagedListOfModel = pagedListFromDb.Select(x => x.AutoMap<T1, T2>());

            var totalItemCount = entities.Count();

            return new StaticPagedList<T2>(pagedListOfModel, pageNumber - 1, itemsPerPage, totalItemCount);
        }
    }
}
