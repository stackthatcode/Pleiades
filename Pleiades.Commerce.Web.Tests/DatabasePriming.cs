using System;
using System.Collections.Generic;
using System.Linq;
using Pleiades.Commerce.Domain.Concrete;
using Pleiades.Commerce.Domain.Interface;
using Pleiades.Commerce.Domain.Model.Users;
using Pleiades.Commerce.Persist;
using Pleiades.Commerce.Persist.Users;
using Pleiades.Framework.Data;
using Pleiades.Framework.Identity.Concrete;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.MembershipProvider.Concrete;
using Pleiades.Framework.MembershipProvider.Interface;
using Pleiades.Framework.MembershipProvider.Model;
using Pleiades.Framework.MembershipProvider.Providers;
using Pleiades.Framework.Utility;
using NUnit.Framework;

namespace Pleiades.Commerce.Web.IntegrationTests
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
            var context = new PleiadesContext(Constants.DatabaseConnString);
            if (context.Database.Exists()) 
                context.Database.Delete();
            // Build Database
            Console.WriteLine("Creating Database for Integration Testing of AggregateUserRepository");
            context.Database.Create();
        }

    }
}
