﻿using System;
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
    [TestFixture]
    public class SizeAndSizeGroupRepositoryTests : FixtureBase
    {       
        [Test]
        public void Empty_And_Repopulate_And_Update_Sizes()
        {
            using (var lifetime = TestContainer.LifetimeScope())
            {
                // Empty + populate data using the Initializer Builder
                var container = lifetime.Resolve<IContainerAdapter>();
                SizeBuilder.EmptyAndRepopulate(container);

                var repository = container.Resolve<IJsonSizeRepository>();
                var unitOfWork = container.Resolve<IUnitOfWork>();

                // All Size Groups to JSON
                var allSizeGroups = repository.RetrieveAll();
                Console.WriteLine(Environment.NewLine + "All Size Groups to JSON");
                Console.WriteLine(JsonConvert.SerializeObject(allSizeGroups, Formatting.Indented));

                // One Size Group to JSON
                var sizeGroupId1 = allSizeGroups[0].Id;
                var sizeGroup = repository.RetrieveByGroup(sizeGroupId1);
                Console.WriteLine(Environment.NewLine + "One Size Group to JSON");
                Console.WriteLine(JsonConvert.SerializeObject(sizeGroup, Formatting.Indented));

                // Update a Size
                var size = sizeGroup.Sizes[1];
                size.Name = size.Name + "333";
                repository.Update(size);
                sizeGroup = repository.RetrieveByGroup(sizeGroupId1);
                Console.WriteLine(Environment.NewLine + "One Size Group to JSON - after UPDATE");
                Console.WriteLine(JsonConvert.SerializeObject(sizeGroup, Formatting.Indented));

                // Delete a Size Group
                repository.DeleteSoft(allSizeGroups[0]);
                unitOfWork.SaveChanges();
                allSizeGroups = repository.RetrieveAll();
                Console.WriteLine(Environment.NewLine + "All Size Groups to JSON after Delete");
                Console.WriteLine(JsonConvert.SerializeObject(allSizeGroups, Formatting.Indented));
            }
        }
    }
}