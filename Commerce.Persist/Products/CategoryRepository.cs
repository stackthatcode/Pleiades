using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Pleiades.Data;
using Pleiades.Data.EF;

namespace Commerce.Persist.Products
{
    public class CategoryRepository: EFGenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbContext context)
            : base(context)
        {
        }

        protected IQueryable<Category> NotDeletedData()
        {
            return this.ReadOnlyData().Where(x => x.Deleted == false);
        }

        public List<Category> RetrieveAllSections()
        {
            return NotDeletedData().Where(x => x.ParentId == null && x.Deleted == false).ToList();
        }

        public List<Category> RetrieveAllCategoriesBySection(int sectionCategoryId)
        {
            return this
                .NotDeletedData()
                .Join(
                    this.ReadOnlyData(), 
                    parent => parent.Id, 
                    child => child.ParentId, 
                    (parent, child) => new { parent, child })
                .Where(x => x.parent.Id == sectionCategoryId || x.parent.ParentId == sectionCategoryId)
                .Select(x => x.child).ToList();
        }

        public List<Category> RetrieveCategoriesByCategory(int categoryId)
        {
            return this.NotDeletedData()
                .Where(x => x.ParentId == categoryId || x.Id == categoryId).ToList();
        }

        public Category RetrieveByIdWriteable(int Id)
        {
            return this.FindFirstOrDefault(x => x.Id == Id);
        }

        public override void Add(Category category)
        {
            category.DateInserted = DateTime.Now;
            category.DateUpdated = DateTime.Now;
            base.Add(category);
        }

        public void Delete(int Id)
        {
            var category = this.RetrieveByIdWriteable(Id);
            category.ParentId = null;
            category.Deleted = true;
        }

        public void SwapParentChild(int parentId, int childId)
        {
            var parent = this.RetrieveByIdWriteable(parentId);
            var child = this.RetrieveByIdWriteable(childId);
            child.ParentId = parent.ParentId;
            parent.ParentId = child.Id;
        }
    }
}
