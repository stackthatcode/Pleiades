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
    /// this Repository exposes the JsonCategory object for representing hierarchical data.
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        DbContext Context { get; set; }

        public CategoryRepository(DbContext context)
        {
            this.Context = context;
        }

        protected IQueryable<Category> Data()
        {
            return Context.Set<Category>().Where(x => x.Deleted == false);
        }

        protected IQueryable<Category> ReadOnlyData()
        {
            return this.Data().AsNoTracking();
        }

        // These are earmarked for porting into Dapper ORM
        public List<JsonCategory> RetrieveAllSectionsOnlyJson()
        {
            return this
                .ReadOnlyData()
                .Where(x => x.ParentId == null && x.Deleted == false)
                .ToList()                
                .ToJsonCategoryList(null);
        }

        public List<JsonCategory> RetrieveBySectionIdJson(int sectionCategoryId)
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
                    .Select(x => x.child)
                    .ToList();

            // Transpose to JSON data
            return categoryData.ToJsonCategoryList(sectionCategoryId);
        }

        public JsonCategory RetrieveByCategoryIdJson(int categoryId)
        {
            // Get the SQL data
            var categoryData = this.ReadOnlyData().Where(x => x.ParentId == categoryId || x.Id == categoryId).ToList();

            // Transpose to JSON data
            return categoryData.ToJsonCategory(categoryId);
        }


        // EF is fine for your everyday CRUD operations
        protected Category Retrieve(int id)
        {
            return this.Data().FirstOrDefault(x => x.Id == id);
        }

        public Func<JsonCategory> Insert(JsonCategory jsonCategory)
        {
            var category = new Category()
              {
                  Name = jsonCategory.Name,
                  SEO = jsonCategory.SEO,
                  ParentId = jsonCategory.ParentId,
              };

            category.DateInserted = DateTime.Now;
            category.DateUpdated = DateTime.Now;
            Context.Set<Category>().Add(category);

            return () => category.ToJsonCategory();
        }

        public void Update(JsonCategory jsonCategory)
        {
            var category = this.Retrieve(jsonCategory.Id.Value);
            category.Name = jsonCategory.Name;
            category.SEO = jsonCategory.SEO;
            category.ParentId = jsonCategory.ParentId;
        }

        public void DeleteSoft(int categoryId)
        {
            var categories = this.Data().Where(x => x.ParentId == categoryId || x.Id == categoryId).ToList();

            foreach (var category in categories)
            {
                //category.ParentId = null;
                category.Deleted = true;
                category.Touch();
            }
        }

        public void SwapParentChild(int parentId, int newParentId)
        {
            var newParent = this.Retrieve(newParentId);
            var parent = this.Retrieve(parentId);

            foreach (var child in this.Data().Where(x => x.ParentId == parentId))
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
}
