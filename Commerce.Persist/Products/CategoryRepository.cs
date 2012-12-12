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

        public List<Category> RetrieveAllSections()
        {
            // Need to add the recursive...?
            return this.FindBy(x => x.ParentId == null && x.Deleted == false).ToList();
        }

        public List<Category> RetrieveAllCategoriesBySection(int sectionCategoryId)
        {
            return this.ReadOnlyData()
                .Join(
                    this.ReadOnlyData(), 
                    parent => parent.Id, 
                    child => child.ParentId, 
                    (parent, child) => new { parent, child })
                .Where(x => x.parent.Id == sectionCategoryId || x.parent.ParentId == sectionCategoryId)
                .Select(x => x.child).ToList();
        }

        public List<Category> RetrieveByParentId(int Id)
        {
            return this.FindBy(x => x.ParentId == Id && x.Deleted == false).ToList();
        }

        public List<Category> RetrieveByParentIdDeep(int Id)
        {
            return this.FindBy(x => x.ParentId == Id && x.Deleted == false).ToList();
        }

        public Category RetrieveById(int Id)
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
            var category = this.RetrieveById(Id);
            category.ParentId = null;
            category.Deleted = true;
        }

        public void SwapParentChild(int parentId, int childId)
        {
            var parent = this.RetrieveById(parentId);
            var child = this.RetrieveById(childId);
            child.ParentId = parent.ParentId;
            parent.ParentId = child.Id;
        }
    }
}
