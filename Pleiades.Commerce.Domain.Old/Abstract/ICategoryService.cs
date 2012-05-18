using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PagedList;
using Pleiades.Commerce.Domain.Old.Model;

namespace Pleiades.Commerce.Domain.Old.Abstract
{
    public interface ICategoryService
    {
        IPagedList<Category> RetrievePage(
            int pageNumber, int pageSize, CategoryServiceSort sort = CategoryServiceSort.Name);

        int RetrievePageNumberById(
            int id, int pageSize, CategoryServiceSort sort = CategoryServiceSort.Name);

        Category Retrieve(int id);
        int Save(Category category);
        void Delete(Category category);
    }
}
