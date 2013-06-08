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
using Commerce.Persist.Concrete;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Lists;
using Commerce.Web;
using Commerce.Web.Plumbing;

namespace Commerce.Initializer.Builders
{
    public class SizeBuilder
    {
        public static void Populate(IContainerAdapter ServiceLocator)
        {
            using (var tx = new TransactionScope())
            {
                Console.WriteLine("Create the default Size Groups and Sizes");

                // Clear everything out
                var sizeRepository = ServiceLocator.Resolve<IGenericRepository<Size>>();
                var sizeGroupRepository = ServiceLocator.Resolve<IGenericRepository<SizeGroup>>();
                sizeRepository.GetAll().ForEach(x => sizeRepository.Delete(x));
                sizeGroupRepository.GetAll().ForEach(x => sizeGroupRepository.Delete(x));

                var unitOfWork = ServiceLocator.Resolve<IUnitOfWork>();
                unitOfWork.SaveChanges();

                // Create the JSON Repos
                var jsonSizeRepository = ServiceLocator.Resolve<IJsonSizeRepository>();

                // Create size groups
                var result0 = jsonSizeRepository.Insert(new JsonSizeGroup { Name = "N/A", Default = true });
                var result1 = jsonSizeRepository.Insert(new JsonSizeGroup { Name = "Default Clothing", Default = false });
                var result2 = jsonSizeRepository.Insert(new JsonSizeGroup { Name = "Men's Shoes", Default = false });
                unitOfWork.SaveChanges();

                var sizeGroup1 = result1();
                var sizeGroup2 = result2();

                // Create sizes
                jsonSizeRepository.Insert(new JsonSize { Name = "S", Description = "Small", SkuCode = "SM", ParentGroupId = sizeGroup1.Id });
                jsonSizeRepository.Insert(new JsonSize { Name = "M", Description = "Medium", SkuCode = "MD", ParentGroupId = sizeGroup1.Id });
                jsonSizeRepository.Insert(new JsonSize { Name = "L", Description = "Large", SkuCode = "LG", ParentGroupId = sizeGroup1.Id });
                jsonSizeRepository.Insert(new JsonSize { Name = "XL", Description = "X-Large", SkuCode = "XL", ParentGroupId = sizeGroup1.Id });
                jsonSizeRepository.Insert(new JsonSize { Name = "XXL", Description = "XX-Large", SkuCode = "XXL", ParentGroupId = sizeGroup1.Id });

                jsonSizeRepository.Insert(new JsonSize { Name = "8", Description = "Size 8", SkuCode = "SZ8", ParentGroupId = sizeGroup2.Id});
                jsonSizeRepository.Insert(new JsonSize { Name = "9", Description = "Size 9", SkuCode = "SZ9", ParentGroupId = sizeGroup2.Id});
                jsonSizeRepository.Insert(new JsonSize { Name = "10", Description = "Size 10", SkuCode = "SZ10", ParentGroupId = sizeGroup2.Id});
                jsonSizeRepository.Insert(new JsonSize { Name = "11", Description = "Size 11", SkuCode = "SZ11", ParentGroupId = sizeGroup2.Id});
                jsonSizeRepository.Insert(new JsonSize { Name = "12", Description = "Size 12", SkuCode = "SZ12", ParentGroupId = sizeGroup2.Id});
                jsonSizeRepository.Insert(new JsonSize { Name = "13", Description = "Size 13", SkuCode = "SZ13", ParentGroupId = sizeGroup2.Id});

                unitOfWork.SaveChanges();
                tx.Complete();
            }
        }
    }
}
