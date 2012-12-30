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

                var repository = ServiceLocator.Resolve<ICategoryRepository>();
                var unitOfWork = ServiceLocator.Resolve<IUnitOfWork>();

                var all = repository.GetAll();
                all.ForEach(x => repository.Delete(x));
                unitOfWork.SaveChanges();

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
                repository.Insert(new Category { ParentId = parent2_2.Id, Name = "Empty Section", SEO = "Empty Section" });
                unitOfWork.SaveChanges();

                var section3 = new Category() { ParentId = null, Name = "Empty Section", SEO = "Empty Section" };
                repository.Insert(section3);
                unitOfWork.SaveChanges();
                tx.Complete();
            }
        }
    }
}
