using System;
using Autofac;
using Newtonsoft.Json;
using NUnit.Framework;
using Pleiades.Application.Data;
using Commerce.Application.Interfaces;
using Commerce.Initializer.Builders;

namespace Commerce.IntegrationTests.BuildersAndRepositories
{
    [TestFixture]
    public class SizeAndSizeGroupTests : FixtureBase
    {       
        [Test]
        public void Empty_And_Repopulate_And_Update_Sizes()
        {
            using (var lifetime = TestContainer.LifetimeScope())
            {
                // Empty + populate data using the Initializer Builder
                lifetime.Resolve<SizeBuilder>().Run();

                var repository = lifetime.Resolve<IJsonSizeRepository>();
                var unitOfWork = lifetime.Resolve<IUnitOfWork>();

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