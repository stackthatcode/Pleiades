using System;
using System.IO;
using System.Configuration;
using Commerce.Persist.Database;
using Pleiades.Application.Utility;
using NUnit.Framework;

namespace Commerce.IntegrationTests
{
    [TestFixture]
    public class FixtureBase
    {
        public static bool recreateDatabaseAndResources = false;
        public static PushMarketContext context = new PushMarketContext();

        public FixtureBase()
        {
            // Clean-out the Database
            if (!FixtureBase.recreateDatabaseAndResources)
            {
                FixtureBase.RecreateDatabase();
                FixtureBase.DestroyResourceFiles();
                FixtureBase.recreateDatabaseAndResources = true;
            }
        }

        public static void RecreateDatabase()
        {
            if (context.Database.Exists())
            {
                Console.WriteLine("Deleting Database for Integration Testing");
                context.Database.Delete();
            }

            Console.WriteLine("Creating Database for Integration Testing");
            context.Database.Create();
        }

        public static void DestroyResourceFiles()
        {
            // Clean-out the Resource Directory
            Console.WriteLine("Deleting Resource Files for Integration Testing");
            var resourceDirectory = ConfigurationManager.AppSettings["ResourceStorage"];
            var directoryInfo = new DirectoryInfo(resourceDirectory);
            directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).ForEach(x => x.Delete());
            directoryInfo.GetDirectories().ForEach(x => x.Delete());
        }

        public static void CleanOutUserData()
        {
            Console.WriteLine("Deleting User Data for Integration Testing");
            context.AggregateUsers.ForEach(x => context.AggregateUsers.Remove(x));
            context.IdentityProfiles.ForEach(x => context.IdentityProfiles.Remove(x));
            context.MembershipUsers.ForEach(x => context.MembershipUsers.Remove(x));
            context.SaveChanges();
        }
    }
}