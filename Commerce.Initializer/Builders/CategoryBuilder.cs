using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Pleiades.Data;
using Pleiades.Injection;
using Pleiades.Utility;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Providers;
using Commerce.Domain.Interfaces;
using Commerce.Domain.Model.Lists;
using Commerce.Persist;
using Commerce.Persist.Security;
using Commerce.WebUI;
using Commerce.WebUI.Plumbing;


namespace Commerce.Initializer.Builders
{
    public class CategoryBuilder
    {
        public static void EmptyAndRepopulate(IContainerAdapter ServiceLocator)
        {
            using (var tx = new TransactionScope())
            {
                Console.WriteLine("Create the default Categories");

                var unitOfWork = ServiceLocator.Resolve<IUnitOfWork>();
                var repository = ServiceLocator.Resolve<IGenericRepository<Category>>();

                var all = repository.GetAll();
                all.ForEach(x => repository.Delete(x));
                unitOfWork.SaveChanges();

                var jsonRepository = ServiceLocator.Resolve<ICategoryRepository>();

                var result1 = jsonRepository.Insert(
                    new JsonCategory() { ParentId = null, Name = "Good Gear Section", SEO = "Men's Section" });
                unitOfWork.SaveChanges();
                var section1 = result1();

                var result1_1 = jsonRepository.Insert(
                    new JsonCategory() { ParentId = section1.Id, Name = "Shoes", SEO = "sample-seo-text" });
                unitOfWork.SaveChanges();
                var parent1_1 = result1_1();

                jsonRepository.Insert(new JsonCategory { ParentId = parent1_1.Id, Name = "Golf Shoes", SEO = "sample-seo-text-4" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent1_1.Id, Name = "Playah Shoes", SEO = "sample-seo-text-5" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent1_1.Id, Name = "Mountain Shoes", SEO = "sample-seo-text-6" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent1_1.Id, Name = "Ass-Kicking Shoes", SEO = "sample-seo-text-7" });
                unitOfWork.SaveChanges();

                var result1_2 = jsonRepository.Insert(
                    new JsonCategory() { ParentId = section1.Id, Name = "Jiu-jitsu Gear", SEO = "sample-seo-text-8" });
                unitOfWork.SaveChanges();
                var parent1_2 = result1_2();

                jsonRepository.Insert(new JsonCategory { ParentId = parent1_2.Id, Name = "Choke-proof Gis", SEO = "sample-seo-text-9" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent1_2.Id, Name = "Belts", SEO = "sample-seo-text-10" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent1_2.Id, Name = "Mouth Guards", SEO = "sample-seo-text-11" });
                unitOfWork.SaveChanges();

                var result1_3 = jsonRepository.Insert(
                    new JsonCategory() { ParentId = section1.Id, Name = "Helmets", SEO = "sample-seo-text-12" });
                unitOfWork.SaveChanges();
                var parent1_3 = result1_3();

                jsonRepository.Insert(new JsonCategory { ParentId = parent1_3.Id, Name = "Open Face", SEO = "sample-seo-text-13" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent1_3.Id, Name = "Viking", SEO = "sample-seo-text-15" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent1_3.Id, Name = "Mouth Guards", SEO = "sample-seo-text-11" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent1_3.Id, Name = "Samurai", SEO = "sample-seo-text-16" });
                unitOfWork.SaveChanges();

                var result2 = jsonRepository.Insert(
                    new JsonCategory() { ParentId = null, Name = "MMA Section", SEO = "Men's Section" });
                unitOfWork.SaveChanges();
                var section2 = result2();

                var result2_1 = jsonRepository.Insert(
                    new JsonCategory() { ParentId = section2.Id, Name = "Boxing", SEO = "sample-seo-text-17" });
                unitOfWork.SaveChanges();
                var parent2_1 = result2_1();

                jsonRepository.Insert(new JsonCategory { ParentId = parent2_1.Id, Name = "Punching Bags", SEO = "sample-seo-text-18" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent2_1.Id, Name = "Gloves", SEO = "sample-seo-text-19" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent2_1.Id, Name = "Mouth Guards", SEO = "sample-seo-text-20" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent2_1.Id, Name = "Shorts", SEO = "sample-seo-text-21" });
                unitOfWork.SaveChanges();

                var result2_2=  jsonRepository.Insert(
                    new JsonCategory() { ParentId = section2.Id, Name = "Paddy Cake", SEO = "sample-seo-text-22" });
                unitOfWork.SaveChanges();
                var parent2_2 = result2_2();

                jsonRepository.Insert(new JsonCategory { ParentId = parent2_2.Id, Name = "Tea", SEO = "sample-seo-text-22" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent2_2.Id, Name = "Spice", SEO = "sample-seo-text-23" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent2_2.Id, Name = "Everything Nice", SEO = "sample-seo-text-23" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent2_2.Id, Name = "Crumpets", SEO = "sample-seo-text-24" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent2_2.Id, Name = "Crumpets X", SEO = "sample-seo-text-24" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent2_2.Id, Name = "Crumpets Y", SEO = "sample-seo-text-24" });
                jsonRepository.Insert(new JsonCategory { ParentId = parent2_2.Id, Name = "Crumpets Z", SEO = "sample-seo-text-24" });
                unitOfWork.SaveChanges();

                var section3 = jsonRepository.Insert(
                    new JsonCategory() { ParentId = null, Name = "Empty Section", SEO = "Empty Section" })();
                unitOfWork.SaveChanges();
                tx.Complete();
            }
        }
    }
}
