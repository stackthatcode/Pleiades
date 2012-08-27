using System;
using Commerce.Persist;
using Commerce.Persist.Security;
using Pleiades.Data;
using Pleiades.Utility;
using NUnit.Framework;

namespace CommerceIntegrationTests
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
    }
}
