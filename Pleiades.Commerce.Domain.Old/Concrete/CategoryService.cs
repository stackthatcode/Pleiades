using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using PagedList;
using Pleiades.Commerce.Domain.Old.Abstract;
using Pleiades.Commerce.Domain.Old.Data;
using Pleiades.Commerce.Domain.Old.Model;
using Pleiades.Framework.Helpers;

namespace Pleiades.Commerce.Domain.Old.Concrete
{
    public class CategoryService : ICategoryService
    {
        public IPagedList<Model.Category> RetrievePage(
                int pageNumber, int pageSize, CategoryServiceSort sort = CategoryServiceSort.Name)
        {
            var datacontext = new PleiadesDBEntities();
            var records = Sort(datacontext.Categories, sort);
            return records.PageFromEntities<Data.Category, Model.Category>(pageNumber, pageSize);
        }

        public int RetrievePageNumberById(
                int categoryId, int pageSize, CategoryServiceSort sort = CategoryServiceSort.Name)
        {
            var datacontext = new PleiadesDBEntities();
            var records = Sort(datacontext.Categories, sort);
            var pageNumber = records.FindPageNumber(pageSize, x => x.Id == categoryId);
            return pageNumber;
        }

        private IQueryable<Data.Category> Sort(IQueryable<Data.Category> records, CategoryServiceSort sort)
        {
            if (sort == CategoryServiceSort.Name)
                return records.OrderBy(x => x.Name);
            if (sort == CategoryServiceSort.Desccription)
                return records.OrderBy(x => x.Description);
            if (sort == CategoryServiceSort.Active)
                return records.OrderBy(x => x.Active);

            throw new NotImplementedException();
        }


        public Model.Category Retrieve(int id)
        {
            var datacontext = new PleiadesDBEntities();
            return datacontext.Categories.First(x => x.Id == id).AutoMap<Data.Category, Model.Category>();
        }

        public int Save(Model.Category category)
        {
            var datacontext = new PleiadesDBEntities();

            var persistentCategory = datacontext.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (persistentCategory == null)
            {
                persistentCategory = new Data.Category();
                persistentCategory.DateCreated = DateTime.Now;
                datacontext.Categories.AddObject(persistentCategory);
            }

            persistentCategory.Name = category.Name;
            persistentCategory.Description = category.Description;
            persistentCategory.Active = category.Active;
            persistentCategory.LastModified = DateTime.Now;
            datacontext.SaveChanges();
            return persistentCategory.Id;
        }

        public void Delete(Model.Category category)
        {
            var datacontext = new PleiadesDBEntities();            
            var persistentCategory = datacontext.Categories.FirstOrDefault(x => x.Id == category.Id);

            if (persistentCategory != null)
            {
                datacontext.Categories.DeleteObject(persistentCategory);
                datacontext.SaveChanges();
            }
        }
    }
}
