using System;
using System.Configuration;
using System.IO;
using Commerce.Application.Database;
using NUnit.Framework;
using Pleiades.App.Utility;

namespace ArtOfGroundFighting.IntegrationTests
{
    [TestFixture]
    public class FixtureBase
    {
        public bool RecreateDatabaseAndResources = true;
        public PushMarketContext Context = new PushMarketContext();

        [TestFixtureSetUp]
        public void FixtureBaseSetup()
        {
            // Clean-out the Database
            if (RecreateDatabaseAndResources && Context.Database.Exists())
            {
                Console.WriteLine("Deleting Database for Integration Testing");
                Context.Database.Delete();
            }

            Console.WriteLine("Creating Database for Integration Testing");
            Context.MasterDatabaseCreate();

            // Clean-out the Resource Directory
            Console.WriteLine("Deleting Resource Files for Integration Testing");
            var resourceDirectory = ConfigurationManager.AppSettings["ResourceStorage"];
            var directoryInfo = new DirectoryInfo(resourceDirectory);
            directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).ForEach(x => x.Delete());
            directoryInfo.GetDirectories().ForEach(x => x.Delete());

            RecreateDatabaseAndResources = true;

            Console.WriteLine("Deleting User Data for Integration Testing");
            Context.AggregateUsers.ForEach(x => Context.AggregateUsers.Remove(x));
            Context.IdentityProfiles.ForEach(x => Context.IdentityProfiles.Remove(x));
            Context.MembershipUsers.ForEach(x => Context.MembershipUsers.Remove(x));
            Context.SaveChanges();
        }
    }
}
