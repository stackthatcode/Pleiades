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
    public class DatabasePriming
    {
        [Test]
        [Ignore]    // Remove this line to refresh database schema
        public void CreateDatabaseHarness()
        {
            RecreateTheDatabase();
        }

        public static void RecreateTheDatabase()
        {
            var context = new PleiadesContext();
            if (context.Database.Exists()) 
                context.Database.Delete();
            // Build Database
            Console.WriteLine("Creating Database for Integration Testing of AggregateUserRepository");
            context.Database.Create();
        }

        public static void InitializeMembership()
        {
            // Prepare Membership Provider - YOU HAVE TO USE THIS, OTHERWISE YOUR REPOSITORY WILL NOT WORK!!!
            PfMembershipRepositoryBroker.RegisterFactory(() => new PfMembershipRepository(new PleiadesContext()));
        }

        public static void CleanOutTheDatabase()
        {
            var context = new PleiadesContext();
            if (!context.Database.Exists())
            {
                // Build Database
                Console.WriteLine("Creating Database for Integration Testing of AggregateUserRepository");
                context.Database.Create();
            }

            // Empty the Membership Users
            context.AggregateUsers.ForEach(x => context.AggregateUsers.Remove(x));
            context.IdentityProfiles.ForEach(x => context.IdentityProfiles.Remove(x));
            context.MembershipUsers.ForEach(x => context.MembershipUsers.Remove(x));
            context.SaveChanges();
        }
    }
}
