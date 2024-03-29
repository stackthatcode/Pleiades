using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Commerce.Application.Database;
using Commerce.Application.Lists.Entities;

namespace Commerce.Application.Lists
{
    /// <summary>
    /// Due to the complexity of storing the hierarchical data in SQL whilst transposing to hierarchial structures,
    /// this ReadOnlyRepository exposes the JsonCategory object for representing hierarchical data.
    /// </summary>
    public class JsonCategoryRepository : IJsonCategoryRepository
    {
        PushMarketContext Context { get; set; }

        public JsonCategoryRepository(PushMarketContext context)
        {
            this.Context = context;
        }

        protected IQueryable<Category> Data()
        {
            return Context.Categories.Where(x => x.Deleted == false);
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

        protected IQueryable<Category> Sections()
        {
            return this.Data().Where(x => x.ParentId == null);
        }


        // These should be earmarked for porting into Dapper ORM
        public List<JsonCategory> RetrieveAllSectionsNoCategories()
        {
            var output = this.Sections().ToList().Select(x => x.ToJson()).ToList();

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

        public List<JsonCategory> RetrieveAllSectionsWithCategories()
        {
            var allCategories = this.ReadOnlyData().ToList();
            var output = new List<JsonCategory>();

            foreach (var section in allCategories.Where(x => x.ParentId == null))
            {
                var jsonSection = section.ToJson();
                output.Add(jsonSection);
                jsonSection.Categories.AddRange(allCategories.ToJsonListWithChildren(section.Id));
            }

            return output;
        }

        public List<JsonCategory> RetrieveAllCategoriesBySectionId(int sectionCategoryId)
        {
            return this.CategoriesBySection(sectionCategoryId)
                .ToList()
                .ToJsonListWithChildren(sectionCategoryId);
        }

        public JsonCategory RetrieveCategoryAndChildrenById(int categoryId)
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
            Touch(category);
        }

        public void DeleteCategory(int categoryId)
        {
            var categories = this.Data().Where(x => x.ParentId == categoryId || x.Id == categoryId).ToList();

            foreach (var category in categories)
            {
                category.Deleted = true;
                Touch(category);
            }

            var products = 
                this.Context.Products
                    .Where(x => x.Category != null)
                    .Where(x => x.Category.Id == categoryId)
                    .ToList();
                    
            foreach (var product in products)
            {
                product.Category = null;
            }
        }

        public void DeleteSection(int sectionId)
        {
            if (this.Sections().Count() <= 1)
            {
                return;
            }

            var categories = this.CategoriesBySection(sectionId);
            
            foreach (var category in categories)
            {
                category.Deleted = true;
                Touch(category);
            }

            var section = this.Retrieve(sectionId);
            section.Deleted = true;
            Touch(section);
        }

        public void SwapParentChild(int parentId, int newParentId)
        {
            var newParent = this.Retrieve(newParentId);
            var parent = this.Retrieve(parentId);

            foreach (var child in this.Data().Where(x => x.ParentId == parentId))
            {
                child.ParentId = newParentId;
                Touch(child);
            }

            // child is easy
            newParent.ParentId = parent.ParentId;
            Touch(newParent);
            parent.ParentId = newParent.Id;
            Touch(parent);
        }

        public void Touch(Category category)
        {
            this.Context.MarkModified(category);
            category.DateUpdated = DateTime.Now;
        }

    }
}