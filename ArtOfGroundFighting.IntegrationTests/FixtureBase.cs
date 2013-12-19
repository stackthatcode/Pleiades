using System;
using System.Configuration;
using Autofac;
using NUnit.Framework;
using Commerce.Application.Database;
using Commerce.Application.File;
using Pleiades.App.Utility;

namespace ArtOfGroundFighting.IntegrationTests
{
    [TestFixture]
    public class FixtureBase
    {
        public bool RecreateDatabaseAndResources = Boolean.Parse(
                    ConfigurationManager.AppSettings["RecreateDatabaseAndResources"]);

        [TestFixtureSetUp]
        public void FixtureBaseSetup()
        {
            using (var scope = TestContainer.LifetimeScope())
            {
                var context = scope.Resolve<PushMarketContext>();

                // Clean-out the Database
                if (RecreateDatabaseAndResources && context.Database.Exists())
                {
                    Console.WriteLine("Deleting Database for Integration Testing");
                    context.Database.Delete();
                }

                if (!context.Database.Exists())
                {
                    Console.WriteLine("Creating Database for Integration Testing");
                    context.MasterDatabaseCreate();
                }
                else
                {
                    Console.WriteLine("Deleting User Data for Integration Testing");
                    context.AggregateUsers.ForEach(x => context.AggregateUsers.Remove(x));
                    context.IdentityProfiles.ForEach(x => context.IdentityProfiles.Remove(x));
                    context.MembershipUsers.ForEach(x => context.MembershipUsers.Remove(x));
                    context.SaveChanges();
                }

                // Clean-out the Resource Directory
                Console.WriteLine("Deleting Resource Files for Integration Testing");
                var fileRepository = scope.Resolve<IFileResourceRepository>();
                fileRepository.NuclearDelete();
            }
        }
    }
}
