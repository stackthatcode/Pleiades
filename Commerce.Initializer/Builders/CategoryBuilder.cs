using System;
using System.Transactions;
using Commerce.Application.Lists;
using Commerce.Application.Lists.Entities;
using Pleiades.Application.Data;
using Pleiades.Application.Helpers;
using Pleiades.Application.Logging;

namespace Commerce.Initializer.Builders
{
    public class CategoryBuilder : IBuilder
    {
        private IUnitOfWork _unitOfWork;
        private IGenericRepository<Category> _repository;
        private IJsonCategoryRepository _jsonRepository;

        public CategoryBuilder(
                IUnitOfWork unitOfWork,
                IGenericRepository<Category> genericRepository,
                IJsonCategoryRepository jsonCategoryRepository)
        {
            _unitOfWork = unitOfWork;
            _repository = genericRepository;
            _jsonRepository = jsonCategoryRepository;
        }

        public void Run()
        {
            using (var tx = new TransactionScope())
            {
                LoggerSingleton.Get().Info("Create the default Categories");

                //var all = _repository.GetAll();
                //all.ForEach(x => _repository.Delete(x));
                //_unitOfWork.SaveChangesToDatabase();

                var result1 = _jsonRepository.Insert(
                    new JsonCategory() { ParentId = null, Name = "Good Gear Section", SEO = "Men's Section" });
                _unitOfWork.SaveChanges();
                var section1 = result1();

                var result1_1 = _jsonRepository.Insert(
                    new JsonCategory() { ParentId = section1.Id, Name = "Shoes", SEO = "sample-seo-text" });
                _unitOfWork.SaveChanges();
                var parent1_1 = result1_1();

                _jsonRepository.Insert(new JsonCategory { ParentId = parent1_1.Id, Name = "Golf Shoes", SEO = "sample-seo-text-4" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent1_1.Id, Name = "Playah Shoes", SEO = "sample-seo-text-5" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent1_1.Id, Name = "Mountain Shoes", SEO = "sample-seo-text-6" });
                _unitOfWork.SaveChanges();

                var result1_2 = _jsonRepository.Insert(
                    new JsonCategory() { ParentId = section1.Id, Name = "Jiu-jitsu Gear", SEO = "sample-seo-text-8" });
                _unitOfWork.SaveChanges();
                var parent1_2 = result1_2();

                _jsonRepository.Insert(new JsonCategory { ParentId = parent1_2.Id, Name = "Choke-proof Gis", SEO = "sample-seo-text-9" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent1_2.Id, Name = "Belts", SEO = "sample-seo-text-10" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent1_2.Id, Name = "Mouth Guards", SEO = "sample-seo-text-11" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent1_2.Id, Name = "Training Dummies", SEO = "sample-seo-text-999" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent1_2.Id, Name = "Head Gear", SEO = "head-gear" });
                _unitOfWork.SaveChanges();

                var result1_3 = _jsonRepository.Insert(
                    new JsonCategory() { ParentId = section1.Id, Name = "Helmets", SEO = "sample-seo-text-12" });
                _unitOfWork.SaveChanges();
                var parent1_3 = result1_3();

                _jsonRepository.Insert(new JsonCategory { ParentId = parent1_3.Id, Name = "Open Face", SEO = "sample-seo-text-13" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent1_3.Id, Name = "Viking", SEO = "sample-seo-text-15" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent1_3.Id, Name = "Mouth Guards", SEO = "sample-seo-text-11" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent1_3.Id, Name = "Samurai", SEO = "sample-seo-text-16" });
                _unitOfWork.SaveChanges();

                var result2 = _jsonRepository.Insert(
                    new JsonCategory() { ParentId = null, Name = "MMA Section", SEO = "Men's Section" });
                _unitOfWork.SaveChanges();
                var section2 = result2();

                var result2_1 = _jsonRepository.Insert(
                    new JsonCategory() { ParentId = section2.Id, Name = "Boxing", SEO = "sample-seo-text-17" });
                _unitOfWork.SaveChanges();
                var parent2_1 = result2_1();

                _jsonRepository.Insert(new JsonCategory { ParentId = parent2_1.Id, Name = "Punching Bags", SEO = "sample-seo-text-18" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent2_1.Id, Name = "Gloves", SEO = "sample-seo-text-19" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent2_1.Id, Name = "Mouth Guards", SEO = "sample-seo-text-20" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent2_1.Id, Name = "Shorts", SEO = "sample-seo-text-21" });
                _unitOfWork.SaveChanges();

                var result2_2=  _jsonRepository.Insert(
                    new JsonCategory() { ParentId = section2.Id, Name = "Paddy Cake", SEO = "sample-seo-text-22" });
                _unitOfWork.SaveChanges();
                var parent2_2 = result2_2();

                _jsonRepository.Insert(new JsonCategory { ParentId = parent2_2.Id, Name = "Tea", SEO = "sample-seo-text-22" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent2_2.Id, Name = "Spice", SEO = "sample-seo-text-23" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent2_2.Id, Name = "Everything Nice", SEO = "sample-seo-text-23" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent2_2.Id, Name = "Crumpets", SEO = "sample-seo-text-24" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent2_2.Id, Name = "Crumpets X", SEO = "sample-seo-text-24" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent2_2.Id, Name = "Crumpets Y", SEO = "sample-seo-text-24" });
                _jsonRepository.Insert(new JsonCategory { ParentId = parent2_2.Id, Name = "Crumpets Z", SEO = "sample-seo-text-24" });
                _unitOfWork.SaveChanges();

                var section3 = _jsonRepository.Insert(
                    new JsonCategory() { ParentId = null, Name = "Empty Section", SEO = "Empty Section" })();
                _unitOfWork.SaveChanges();
                tx.Complete();
            }
        }
    }
}
