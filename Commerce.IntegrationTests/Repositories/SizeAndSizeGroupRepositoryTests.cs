using System;
using System.Linq;
using System.Transactions;
using Autofac;
using NUnit.Framework;
using Pleiades.Application.Data;
using Pleiades.Application.Injection;
using Pleiades.Application;
using Pleiades.Application.EF;
using Pleiades.Application.Utility;
using Commerce.Persist;
using Commerce.Persist.Concrete;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Lists;
using Commerce.Initializer.Builders;
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
                SizeBuilder.Populate(container);

                var repository = container.Resolve<IJsonSizeRepository>();
                var unitOfWork = container.Resolve<IUnitOfWork>();

                // All Size Groups to JSON
                var allSizeGroups = repository.RetrieveAll(false);
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
                allSizeGroups = repository.RetrieveAll(false);
                Console.WriteLine(Environment.NewLine + "All Size Groups to JSON after Delete");
                Console.WriteLine(JsonConvert.SerializeObject(allSizeGroups, Formatting.Indented));
            }
        }
    }
}