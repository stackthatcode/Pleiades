using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac;
using Pleiades.Data;
using Pleiades.Utility;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;
using Commerce.Domain.Interface;
using Commerce.Persist;
using Commerce.Persist.Security;
using Commerce.WebUI;

namespace CommerceInitializer
{
    class Program
    {
        static PleiadesContext DbContext { get; set; }
        static ILifetimeScope LifetimeScope { get; set; }

        static void Main(string[] args)
        {
            // Components
            RegisterDIContainer();
            MembershipRepositoryShim.SetFactory();

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
            builder.RegisterModule<CommerceWebUIModule>();
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