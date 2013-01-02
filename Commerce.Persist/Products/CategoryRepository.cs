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

        protected IQueryable<Category> CategoriesBySection(int sectionId)
        {
            return
                this.ReadOnlyData()
                    .Join(
                        this.ReadOnlyData(),
                        parent => parent.Id,
                        child => child.ParentId,
                        (parent, child) => new { parent, child })
                    .Where(x => x.parent.Id == sectionId || x.parent.ParentId == sectionId)
                    .Select(x => x.child);
        }


        // These should be earmarked for porting into Dapper ORM
        public List<JsonCategory> RetrieveSectionsOnlyJson()
        {
            var output = this.Data().Where(x => x.ParentId == null).ToList().Select(x => x.ToJson()).ToList();

            var parentChildPairs = this.ReadOnlyData()
                    .Join(
                        this.ReadOnlyData(),
                        parent => parent.Id,
                        child => child.ParentId,
                        (Parent, Child) => new { Parent, Child })
                    .ToList();
                    
            foreach (var section in output)
            {
                section.NumberOfCategories =
                    parentChildPairs.Count(x => x.Parent.Id == section.Id || x.Parent.ParentId == section.Id);
            }

            return output.ToList(); 
        }

        public List<JsonCategory> RetrieveBySectionIdJson(int sectionCategoryId)
        {
            return this.CategoriesBySection(sectionCategoryId)
                .ToList()
                .ToJsonListWithChildren(sectionCategoryId);
        }

        public JsonCategory RetrieveByCategoryIdJson(int categoryId)
        {
            // Get the SQL data
            var categoryData = this.ReadOnlyData().Where(x => x.ParentId == categoryId || x.Id == categoryId).ToList();

            // Transpose to JSON data
            return categoryData.ToJsonWithChildren(categoryId);
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

            return () => category.ToJson();
        }

        public void Update(JsonCategory jsonCategory)
        {
            var category = this.Retrieve(jsonCategory.Id.Value);
            category.Name = jsonCategory.Name;
            category.SEO = jsonCategory.SEO;
            category.ParentId = jsonCategory.ParentId;
        }

        public void DeleteCategorySoft(int categoryId)
        {
            var categories = this.Data().Where(x => x.ParentId == categoryId || x.Id == categoryId).ToList();

            foreach (var category in categories)
            {
                category.Deleted = true;
                category.Touch();
            }
        }

        public void DeleteSectionSoft(int sectionId)
        {
            var categories = this.CategoriesBySection(sectionId);
            
            foreach (var category in categories)
            {
                category.Deleted = true;
                category.Touch();
            }

            var section = this.Retrieve(sectionId);
            section.Deleted = true;
            section.Touch();
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