using System;
using System.Transactions;
using Commerce.Application.Lists;
using Commerce.Application.Lists.Entities;
using Pleiades.App.Data;
using Pleiades.App.Logging;
using Pleiades.App.Utility;

namespace Commerce.Initializer.Builders
{
    public class SizeBuilder : IBuilder
    {
        private IUnitOfWork _unitOfWork;
        private IGenericRepository<Size> _sizeRepository;
        private IGenericRepository<SizeGroup> _sizeGroupRepository;
        private IJsonSizeRepository _jsonSizeRepository;

        public SizeBuilder(IUnitOfWork unitOfWork,
            IGenericRepository<Size> sizeRepository,
            IGenericRepository<SizeGroup> sizeGroupRepository,
            IJsonSizeRepository jsonSizeRepository)            
        {
            _unitOfWork = unitOfWork;
            _sizeRepository = sizeRepository;
            _sizeGroupRepository = sizeGroupRepository;
            _jsonSizeRepository = jsonSizeRepository;
        }

        public void Run()
        {
            using (var tx = new TransactionScope())
            {
                LoggerSingleton.Get().Info("Create the default Size Groups and Sizes");

                // Clear everything out
                //_sizeRepository.GetAll().ForEach(x => _sizeRepository.Delete(x));
                //_sizeGroupRepository.GetAll().ForEach(x => _sizeGroupRepository.Delete(x));
                //_unitOfWork.SaveChangesToDatabase();

                // Create size groups
                var result0 = _jsonSizeRepository.Insert(new JsonSizeGroup { Name = "N/A", Default = true });
                var result1 = _jsonSizeRepository.Insert(new JsonSizeGroup { Name = "Default Clothing", Default = false });
                var result2 = _jsonSizeRepository.Insert(new JsonSizeGroup { Name = "Men's Shoes", Default = false });
                _unitOfWork.SaveChanges();

                var sizeGroup1 = result1();
                var sizeGroup2 = result2();

                // Create sizes
                _jsonSizeRepository.Insert(new JsonSize { Name = "S", Description = "Small", SkuCode = "SM", ParentGroupId = sizeGroup1.Id });
                _jsonSizeRepository.Insert(new JsonSize { Name = "M", Description = "Medium", SkuCode = "MD", ParentGroupId = sizeGroup1.Id });
                _jsonSizeRepository.Insert(new JsonSize { Name = "L", Description = "Large", SkuCode = "LG", ParentGroupId = sizeGroup1.Id });
                _jsonSizeRepository.Insert(new JsonSize { Name = "XL", Description = "X-Large", SkuCode = "XL", ParentGroupId = sizeGroup1.Id });
                _jsonSizeRepository.Insert(new JsonSize { Name = "XXL", Description = "XX-Large", SkuCode = "XXL", ParentGroupId = sizeGroup1.Id });

                _jsonSizeRepository.Insert(new JsonSize { Name = "8", Description = "Size 8", SkuCode = "SZ8", ParentGroupId = sizeGroup2.Id});
                _jsonSizeRepository.Insert(new JsonSize { Name = "9", Description = "Size 9", SkuCode = "SZ9", ParentGroupId = sizeGroup2.Id});
                _jsonSizeRepository.Insert(new JsonSize { Name = "10", Description = "Size 10", SkuCode = "SZ10", ParentGroupId = sizeGroup2.Id});
                _jsonSizeRepository.Insert(new JsonSize { Name = "11", Description = "Size 11", SkuCode = "SZ11", ParentGroupId = sizeGroup2.Id});
                _jsonSizeRepository.Insert(new JsonSize { Name = "12", Description = "Size 12", SkuCode = "SZ12", ParentGroupId = sizeGroup2.Id});
                _jsonSizeRepository.Insert(new JsonSize { Name = "13", Description = "Size 13", SkuCode = "SZ13", ParentGroupId = sizeGroup2.Id});

                _unitOfWork.SaveChanges();
                tx.Complete();
            }
        }
    }
}
