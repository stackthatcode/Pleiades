using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Pleiades.Data;
using Pleiades.Injection;
using Pleiades.Utility;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;
using Pleiades.Web.Security.Providers;
using Commerce.Persist.Interfaces;
using Commerce.Persist.Model.Lists;
using Commerce.Persist.Concrete;
using Commerce.WebUI;
using Commerce.WebUI.Plumbing;

namespace Commerce.Initializer.Builders
{
    public class RootUserBuilder
    {
        public static void CreateTheSupremeUser(IContainerAdapter ServiceLocator)
        {
            Console.WriteLine("Create the default Root User");

            var userService = ServiceLocator.Resolve<IAggregateUserService>();
            var userRepository = ServiceLocator.Resolve<IAggregateUserRepository>();
            var users = userRepository.Retreive(new List<UserRole>() { UserRole.Supreme });

            if (users.ToList().Count() < 1)
            {
                var membershipService = ServiceLocator.Resolve<IMembershipService>();
                var membershipUserName = membershipService.GetUserNameByEmail("aleksjones@gmail.com");
                if (membershipUserName != null)
                {
                    membershipService.DeleteUser(membershipUserName);
                }

                var identityuser1 = new CreateOrModifyIdentityRequest
                {
                    AccountStatus = AccountStatus.Active,
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