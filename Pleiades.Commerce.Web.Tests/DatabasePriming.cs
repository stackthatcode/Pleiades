using System;
using Pleiades.Commerce.Persist;
using Pleiades.Commerce.Persist.Security;
using Pleiades.Framework.Data;
using Pleiades.Framework.Utility;
using NUnit.Framework;

namespace Pleiades.Commerce.IntegrationTests
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
