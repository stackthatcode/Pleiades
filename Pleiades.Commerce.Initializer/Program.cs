using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac;
using Pleiades.Framework.Data;
using Pleiades.Framework.Identity.Model;
using Pleiades.Framework.MembershipProvider.Model;
using Pleiades.Framework.Utility;
using Pleiades.Commerce.Domain;
using Pleiades.Commerce.Domain.Interface;
using Pleiades.Commerce.Persist;
using Pleiades.Commerce.Persist.Users;

namespace Pleiades.Commerce.Initializer
{
    class Program
    {
        static PleiadesContext DbContext { get; set; }
        static ILifetimeScope LifetimeScope { get; set; }

        static void Main(string[] args)
        {
            // Components
            RegisterDIContainer();
            PfMembershipShimInit.SetFactory();

            // Data
            InitializeDatabase();

            // Application Initialization
            CreateTheSupremeUser();
        }

        public static void InitializeDatabase()
        {
            DbContext = new PleiadesContext();
            if (!DbContext.Database.Exists())
            {
                DbContext.Database.Create();

                // Build Database
                Console.WriteLine("Creating Database for Pleiades");
            }
        }

        public static void RegisterDIContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<CommerceRootModule>();
            var container = builder.Build();
            LifetimeScope = container.BeginLifetimeScope();
        }

        public static void CreateTheSupremeUser()
        {
            var userService = LifetimeScope.Resolve<IAggregateUserService>();
            var userRepository = LifetimeScope.Resolve<IAggregateUserRepository>();
            var users = userRepository.Retreive(new List<UserRole>() { UserRole.Supreme });

            if (users.ToList().Count() < 1)
            {
                var identityuser1 = new CreateOrModifyIdentityUserRequest
                {
                    AccountLevel = AccountLevel.NotApplicable,
                    UserRole = UserRole.Supreme,
                    FirstName = "Master",
                    LastName = "Pleiades",
                };

                var membershipuser1 = new CreateNewMembershipUserRequest
                {
                    Email = "aleksjones@gmail.com",
                    Password = "123456",
                    IsApproved = true,
                    PasswordQuestion = "First Karate Teacher",
                    PasswordAnswer = "Sugiyama",
                };

                PleiadesMembershipCreateStatus outstatus1;
                userService.Create(membershipuser1, identityuser1, out outstatus1);

                if (outstatus1 != PleiadesMembershipCreateStatus.Success)
                {
                    throw new Exception("Failed to create Supreme User");
                }
            }
        }
    }
}