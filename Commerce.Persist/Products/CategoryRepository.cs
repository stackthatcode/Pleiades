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
            return this.FindBy(x => x.ParentId == null && x.Deleted == false).ToList();
        }

        public List<Category> RetrieveAllCategoriesBySection(int sectionCategoryId)
        {
            return this.FindBy(x => x.ParentId == sectionCategoryId && x.Deleted == false).ToList();
        }

        public List<Category> RetrieveByParentId(int Id)
        {
            return this.FindBy(x => x.ParentId == Id && x.Deleted == false).ToList();
        }

        public Category RetrieveById(int Id)
        {
            return this.FindFirstOrDefault(x => x.Id == Id);
        }

        public void Delete(int Id)
        {
            var category = this.RetrieveById(Id);
            category.ParentId = null;
            category.Deleted = true;
        }

        public void SwapParentChild(int parentId, int childId)
        {
            throw new NotImplementedException();
        }
    }
}
