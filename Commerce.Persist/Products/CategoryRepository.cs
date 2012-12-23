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
    /// <summary>
    /// Due to the complexity of storing the hierarchical data in SQL whilst transposing to hierarchial structures,
    /// this Repository hides the SqlCategory object and uses Category object to communicate with the upper layers.
    /// </summary>
    public class CategoryRepository: EFGenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbContext context)
            : base(context)
        {
        }

        // TODO: this actually belongs in another Repository
        protected override IQueryable<Category> ReadOnlyData()
        {
            return base.ReadOnlyData().Where(x => x.Deleted == false);
        }

        public List<JsonCategory> RetrieveAllSectionCategories()
        {
            return this.ReadOnlyData()
                .Where(x => x.ParentId == null && x.Deleted == false)
                .ToList()
                .ToJsonCategoryList(null);
        }

        public List<JsonCategory> RetrieveJsonBySection(int sectionCategoryId)
        {
            // Get the SQL data
            var categoryData =
                this.ReadOnlyData()
                    .Join(
                        this.ReadOnlyData(), 
                        parent => parent.Id, 
                        child => child.ParentId, 
                        (parent, child) => new { parent, child })
                    .Where(x => x.parent.Id == sectionCategoryId || x.parent.ParentId == sectionCategoryId)
                    .Select(x => x.child).ToList();

            // Transpose to JSON data
            return categoryData.ToJsonCategoryList(sectionCategoryId);
        }

        public JsonCategory RetrieveJsonById(int categoryId)
        {
            // Get the SQL data
            var categoryData =
                this.ReadOnlyData().Where(x => x.ParentId == categoryId || x.Id == categoryId).ToList();

            // Transpose to JSON data
            return categoryData.ToJsonCategory(categoryId);
        }

        public Category RetrieveWriteable(int categoryId)
        {
            return this.FindFirstOrDefault(x => x.Id == categoryId);
        }

        public override void Insert(Category category)
        {
            category.DateInserted = DateTime.Now;
            category.DateUpdated = DateTime.Now;
            base.Insert(category);
        }

        public void DeleteSoft(int categoryId)
        {
            var categories = this.FindBy(x => x.ParentId == categoryId || x.Id == categoryId).ToList();
            foreach (var category in categories)
            {
                category.ParentId = null;
                category.Deleted = true;
                category.Touch();
            }
        }

        // This is broken -- must fix
        public void SwapParentChild(int parentId, int newParentId)
        {
            var newParent = this.RetrieveWriteable(newParentId);
            var parent = this.RetrieveWriteable(parentId);

            foreach (var child in this.FindBy(x => x.ParentId == parentId))
            {
                child.ParentId = newParentId;
                child.Touch();
            }

            // child is easy
            newParent.ParentId = parent.ParentId;
            newParent.Touch();
            parent.ParentId = newParent.Id;
            parent.Touch();
        }
    }

    public static class CategoryRepositoryExtensions
    {
        public static List<JsonCategory> ToJsonCategoryList(this List<Category> domainCategoryList, int? parentId)
        {
            var output = new List<JsonCategory>();
            foreach (var domainCategory in domainCategoryList.Where(x => x.ParentId == parentId))
            {
                var modelCategory = ToJsonCategory(domainCategoryList, domainCategory.Id);
                output.Add(modelCategory);
            }

            return output;
        }

        public static JsonCategory ToJsonCategory(this List<Category> domainCategoryList, int? Id)
        {
            var domainCategory = domainCategoryList.First(x => x.Id == Id);
            return new JsonCategory
            {
                Id = domainCategory.Id,
                ParentId = domainCategory.ParentId,
                Name = domainCategory.Name,
                SEO = domainCategory.SEO,
                Categories = ToJsonCategoryList(domainCategoryList, domainCategory.Id),
            };
        }
    }
}
