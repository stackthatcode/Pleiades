using System;
using Pleiades.Data;
using Pleiades.Web.Security.Providers;
using Pleiades.Utility;
using Commerce.Persist;
using Commerce.Persist.Security;
using NUnit.Framework;

namespace Commerce.IntegrationTests
{
    [TestFixture]
    public class FixtureBase
    {
        public static bool recreatedDatabase = false;
        public static PleiadesContext context = new PleiadesContext();

        public FixtureBase()
        {
            if (!FixtureBase.recreatedDatabase)
            {
                FixtureBase.RecreateDatabase();
                FixtureBase.recreatedDatabase = true;
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