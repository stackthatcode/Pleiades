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
    public class SizeBuilder
    {
        public static void EmptyAndRepopulate(IContainerAdapter ServiceLocator)
        {
            using (var tx = new TransactionScope())
            {
                Console.WriteLine("Create the default Size Groups and Sizes");

                var sizeRepository = ServiceLocator.Resolve<ISizeRepository>();
                var sizeGroupRepository = ServiceLocator.Resolve<ISizeGroupRepository>();

                // Clear everything out
                sizeRepository.GetAll().ForEach(x => sizeRepository.Delete(x));
                sizeGroupRepository.GetAll().ForEach(x => sizeGroupRepository.Delete(x));

                var unitOfWork = ServiceLocator.Resolve<IUnitOfWork>();
                unitOfWork.SaveChanges();

                // Create size groups
                var sizeGroup1 = new SizeGroup { Name = "Default Clothing" };
                var sizeGroup2 = new SizeGroup { Name = "Men's Shoes" };

                sizeGroupRepository.Insert(sizeGroup1);
                sizeGroupRepository.Insert(sizeGroup2);
                unitOfWork.SaveChanges();

                // Create sizes
                sizeRepository.Insert(new Size { Name = "S", Description = "Small", SkuCode = "SM", SizeGroup = sizeGroup1 });
                sizeRepository.Insert(new Size { Name = "M", Description = "Medium", SkuCode = "MD", SizeGroup = sizeGroup1 });
                sizeRepository.Insert(new Size { Name = "L", Description = "Large", SkuCode = "LG", SizeGroup = sizeGroup1 });
                sizeRepository.Insert(new Size { Name = "XL", Description = "X-Large", SkuCode = "XL", SizeGroup = sizeGroup1 });
                sizeRepository.Insert(new Size { Name = "XXL", Description = "XX-Large", SkuCode = "XXL", SizeGroup = sizeGroup1 });

                sizeRepository.Insert(new Size { Name = "8", Description = "Size 8", SkuCode = "SZ8", SizeGroup = sizeGroup2 });
                sizeRepository.Insert(new Size { Name = "9", Description = "Size 9", SkuCode = "SZ9", SizeGroup = sizeGroup2 });
                sizeRepository.Insert(new Size { Name = "10", Description = "Size 10", SkuCode = "SZ10", SizeGroup = sizeGroup2 });
                sizeRepository.Insert(new Size { Name = "11", Description = "Size 11", SkuCode = "SZ11", SizeGroup = sizeGroup2 });
                sizeRepository.Insert(new Size { Name = "12", Description = "Size 12", SkuCode = "SZ12", SizeGroup = sizeGroup2 });
                sizeRepository.Insert(new Size { Name = "13", Description = "Size 13", SkuCode = "SZ13", SizeGroup = sizeGroup2 });

                unitOfWork.SaveChanges();
                tx.Complete();
            }
        }
    }
}
