using System;
using System.Linq;
using System.Transactions;
using Autofac;
using NUnit.Framework;
using Pleiades.Data;
using Pleiades.Data.EF;
using Pleiades.Injection;
using Pleiades.Utility;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Initializer.Builders;
using Commerce.Persist;
using Commerce.Persist.Security;
using Newtonsoft.Json;

namespace Commerce.IntegrationTests.Repositories
{
    public class CategoryRepositoryTests
    {
        [Test]
        public void DoStuffWithCategoriesAndBeHappy()
        {
            // Empty + populate data using the Initializer Builder
            var container = TestContainer.LifetimeScope().Resolve<IContainerAdapter>();
            CategoryBuilder.EmptyAndRepopulate(container); 
            
            Console.WriteLine();

            var lifetime = TestContainer.LifetimeScope();
            var repository = lifetime.Resolve<ICategoryRepository>();
            var unitOfWork = lifetime.Resolve<IUnitOfWork>();

            // Show all the parent Sections
            Console.WriteLine("Displaying All Sections");
            var allSections = repository.RetrieveAllSectionCategories();
            allSections.ForEach(x => Console.WriteLine(x.Id + " " + x.Name + " " + x.SEO));
            Console.WriteLine();


            // Show all the Categories under first Section
            var sectionId = allSections[0].Id;
            Console.WriteLine("Displaying All Categories under Section {0}", sectionId);
            var categoriesUnderSection = repository.RetrieveJsonBySection(sectionId.Value);
            categoriesUnderSection.ForEach(x => Console.WriteLine(x.Id + " " + x.ParentId + " " + x.Name + " " + x.SEO));
            Console.WriteLine();


            // Show all the Categories in the first child Categor in the first Section
            var categoryId = categoriesUnderSection[0].Id;
            Console.WriteLine("Displaying All Categories under SectionId {0} and CategoryId {1}", sectionId, categoryId);
            var category = repository.RetrieveJsonById(categoriesUnderSection[0].Id.Value);
            Console.WriteLine();


            // Update then Category            
            Console.WriteLine("Updating Category");
            var categoryWriteable = repository.RetrieveWriteable(category.Id.Value);
            categoryWriteable.Name = "HEllo" + new Random().Next(100);
            unitOfWork.SaveChanges();


            // Show all the Categories in the first child Categor in the first Section, again
            Console.WriteLine("Displaying All Categories under SectionId {0} and CategoryId {1}", sectionId, categoryId);
            var categories2 = repository.RetrieveJsonBySection(categoriesUnderSection[0].Id.Value);
            categories2.ForEach(x => Console.WriteLine(x.Id + " " + x.ParentId + " " + x.Name + " " + x.SEO));
            Console.WriteLine();
        }
    }
}