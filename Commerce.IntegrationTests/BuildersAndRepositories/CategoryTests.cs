using System;
using Autofac;
using Commerce.Application.Lists;
using Newtonsoft.Json;
using NUnit.Framework;
using Pleiades.Application.Data;
using Commerce.Initializer.Builders;

namespace Commerce.IntegrationTests.BuildersAndRepositories
{
    public class CategoryTests : FixtureBase
    {
        [Test]
        public void Empty_And_Repopulate_And_Update_Categories()
        {
            using (var lifetime = TestContainer.LifetimeScope())
            {
                // Empty + populate data using the Initializer Builder
                lifetime.Resolve<CategoryBuilder>().Run();

                Console.WriteLine();
                var repository = lifetime.Resolve<IJsonCategoryRepository>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

                // Show all the parent Sections
                Console.WriteLine("Displaying All Sections");

                var allSections = repository.RetrieveAllSectionsNoCategories();
                allSections.ForEach(x => Console.WriteLine(x.Id + " " + x.Name + " " + x.SEO + " " + x.NumberOfCategories));
                Console.WriteLine();


                // Show all the Categories under first Section
                var sectionId = allSections[0].Id;
                Console.WriteLine("Displaying All Categories under Section {0}", sectionId);

                var categoriesUnderSection = repository.RetrieveAllCategoriesBySectionId(sectionId.Value);
                categoriesUnderSection.ForEach(x => Console.WriteLine(x.Id + " " + x.ParentId + " " + x.Name + " " + x.SEO));                
                Console.WriteLine();


                // Now, let's spit it out in JSON
                Console.WriteLine("Displaying All Categories under Section {0} in JSON", sectionId);
                var sz = JsonConvert.SerializeObject(categoriesUnderSection, Formatting.Indented);
                Console.WriteLine(sz);


                // Show all the Categories in the first child Categor in the first Section
                var categoryId = categoriesUnderSection[0].Id;
                Console.WriteLine("Displaying All Categories under SectionId {0} and CategoryId {1}", sectionId, categoryId);
                
                var category = repository.RetrieveCategoryAndChildrenById(categoriesUnderSection[0].Id.Value);
                Console.WriteLine();


                // Update then Category            
                Console.WriteLine("Updating Category");
                category.Name = "HEllo" + new Random().Next(100);
                repository.Update(category);
                unitOfWork.SaveChanges();


                // Show all the Categories in the first child Categor in the first Section, again
                Console.WriteLine("Displaying All Categories under SectionId {0} and CategoryId {1}", sectionId, categoryId);
                var categories2 = repository.RetrieveAllCategoriesBySectionId(categoriesUnderSection[0].Id.Value);
                categories2.ForEach(x => Console.WriteLine(x.Id + " " + x.ParentId + " " + x.Name + " " + x.SEO));
                Console.WriteLine();
            }
        }
    }
}