using System;
using System.Linq;
using System.Transactions;
using NUnit.Framework;
using Pleiades.Data;
using Pleiades.Data.EF;
using Pleiades.Injection;
using Pleiades.Utility;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Persist;
using Commerce.Persist.Security;
using Newtonsoft.Json;

namespace Commerce.IntegrationTests.Repositories
{
    public class CategoryRepositoryTests
    {
        IContainerAdapter _container;

        //[TestFixtureSetUp]
        public void Setup()
        {
            using (var tx = new TransactionScope())
            {
                _container = IntegrationTestsModule.Container();

                // Empty the test data
                TestPrimer.CleanOutCategoryData();

                var repository = _container.Resolve<ICategoryRepository>();
                var unitOfWork = _container.Resolve<IUnitOfWork>();

                var section1 = new Category() { ParentId = null, Name = "Good Gear Section", SEO = "Men's Section" };
                repository.Insert(section1);
                unitOfWork.SaveChanges();

                var parent1_1 = new Category() { ParentId = section1.Id, Name = "Shoes", SEO = "sample-seo-text" };
                repository.Insert(parent1_1);
                unitOfWork.SaveChanges();

                repository.Insert(new Category { ParentId = parent1_1.Id, Name = "Golf Shoes", SEO = "sample-seo-text-4" });
                repository.Insert(new Category { ParentId = parent1_1.Id, Name = "Playah Shoes", SEO = "sample-seo-text-5" });
                repository.Insert(new Category { ParentId = parent1_1.Id, Name = "Mountain Shoes", SEO = "sample-seo-text-6" });
                repository.Insert(new Category { ParentId = parent1_1.Id, Name = "Ass-Kicking Shoes", SEO = "sample-seo-text-7" });
                unitOfWork.SaveChanges();

                var parent1_2 = new Category() { ParentId = section1.Id, Name = "Jiu-jitsu Gear", SEO = "sample-seo-text-8" };
                repository.Insert(parent1_2);
                unitOfWork.SaveChanges();

                repository.Insert(new Category { ParentId = parent1_2.Id, Name = "Choke-proof Gis", SEO = "sample-seo-text-9" });
                repository.Insert(new Category { ParentId = parent1_2.Id, Name = "Belts", SEO = "sample-seo-text-10" });
                repository.Insert(new Category { ParentId = parent1_2.Id, Name = "Mouth Guards", SEO = "sample-seo-text-11" });
                unitOfWork.SaveChanges();

                var parent1_3 = new Category() { ParentId = section1.Id, Name = "Helmets", SEO = "sample-seo-text-12" };
                repository.Insert(parent1_3);
                unitOfWork.SaveChanges();

                repository.Insert(new Category { ParentId = parent1_3.Id, Name = "Open Face", SEO = "sample-seo-text-13" });
                repository.Insert(new Category { ParentId = parent1_3.Id, Name = "Viking", SEO = "sample-seo-text-15" });
                repository.Insert(new Category { ParentId = parent1_3.Id, Name = "Mouth Guards", SEO = "sample-seo-text-11" });
                repository.Insert(new Category { ParentId = parent1_3.Id, Name = "Samurai", SEO = "sample-seo-text-16" });
                unitOfWork.SaveChanges();

                var section2 = new Category() { ParentId = null, Name = "MMA Section", SEO = "Men's Section" };
                repository.Insert(section2);

                unitOfWork.SaveChanges();

                var parent2_1 = new Category() { ParentId = section2.Id, Name = "Boxing", SEO = "sample-seo-text-17" };
                repository.Insert(parent2_1);
                unitOfWork.SaveChanges();

                repository.Insert(new Category { ParentId = parent2_1.Id, Name = "Punching Bags", SEO = "sample-seo-text-18" });
                repository.Insert(new Category { ParentId = parent2_1.Id, Name = "Gloves", SEO = "sample-seo-text-19" });
                repository.Insert(new Category { ParentId = parent2_1.Id, Name = "Mouth Guards", SEO = "sample-seo-text-20" });
                repository.Insert(new Category { ParentId = parent2_1.Id, Name = "Shorts", SEO = "sample-seo-text-21" });
                unitOfWork.SaveChanges();

                var parent2_2 = new Category() { ParentId = section2.Id, Name = "Paddy Cake", SEO = "sample-seo-text-22" };
                repository.Insert(parent2_2);
                unitOfWork.SaveChanges();

                repository.Insert(new Category { ParentId = parent2_2.Id, Name = "Tea", SEO = "sample-seo-text-22" });
                repository.Insert(new Category { ParentId = parent2_2.Id, Name = "Spice", SEO = "sample-seo-text-23" });
                repository.Insert(new Category { ParentId = parent2_2.Id, Name = "Everything Nice", SEO = "sample-seo-text-23" });
                repository.Insert(new Category { ParentId = parent2_2.Id, Name = "Crumpets", SEO = "sample-seo-text-24" });
                repository.Insert(new Category { ParentId = parent2_2.Id, Name = "Crumpets X", SEO = "sample-seo-text-24" });
                repository.Insert(new Category { ParentId = parent2_2.Id, Name = "Crumpets Y", SEO = "sample-seo-text-24" });
                repository.Insert(new Category { ParentId = parent2_2.Id, Name = "Crumpets Z", SEO = "sample-seo-text-24" });
                unitOfWork.SaveChanges();

                repository.Insert(new Category { ParentId = null, Name = "Empty Section", SEO = "Empty Section" });
                unitOfWork.SaveChanges();

                tx.Complete();
            }
        }

        [Test]
        public void DoStuffWithCategoriesAndBeHappy()
        {
            _container = IntegrationTestsModule.Container();

            var repository = this._container.Resolve<ICategoryRepository>();
            var unitOfWork = this._container.Resolve<IUnitOfWork>();

            Console.WriteLine("Displaying All Sections");
            var allSections = repository.RetrieveAllSectionCategories();
            allSections.ForEach(x => Console.WriteLine(x.Id + " " + x.Name + " " + x.SEO));
            Console.WriteLine();

            var sectionId = allSections[0].Id;
            Console.WriteLine("Displaying All Categories under Section {0}", sectionId);
            var categoriesUnderSection = repository.RetrieveJsonBySection(sectionId );
            categoriesUnderSection.ForEach(x => Console.WriteLine(x.Id + " " + x.ParentId + " " + x.Name + " " + x.SEO));
            Console.WriteLine();

            // Replace this fluff with the JSON method

            //var categoryId = categoriesUnderSection[0].Id;
            //Console.WriteLine("Displaying All Categories under SectionId {0} and CategoryId {1}", sectionId, categoryId);
            //var categories = repository.RetrieveCategoryDeep(categoriesUnderSection[0].Id);
            //categories.ForEach(x => Console.WriteLine(x.Id + " " + x.ParentId + " " + x.Name + " " + x.SEO));
            //Console.WriteLine();

            //Console.WriteLine("Updating Category");
            //var category = repository.RetrieveWriteable(categories[0].Id);
            //category.Name = "HEllo" + new Random().Next(100);
            //unitOfWork.SaveChanges();

            //Console.WriteLine("Displaying All Categories under SectionId {0} and CategoryId {1}", sectionId, categoryId);
            //var categories2 = repository.RetrieveCategoryDeep(categoriesUnderSection[0].Id);
            //categories2.ForEach(x => Console.WriteLine(x.Id + " " + x.ParentId + " " + x.Name + " " + x.SEO));
            //Console.WriteLine();

            //var model = categoriesUnderSection.ToJsonCategoryList(allSections[0].Id);
            //Console.WriteLine("JSON Representation of Object");
            //Console.WriteLine(JsonConvert.SerializeObject(model));
        }
    }
}