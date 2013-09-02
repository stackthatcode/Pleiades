using System;
using System.Collections.Generic;
using System.Linq;
using Pleiades.Application.Logging;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Security.Interface;

namespace Commerce.Initializer.Builders
{
    public class UserBuilder : IBuilder
    {
        private IAggregateUserService _userService;
        private IReadOnlyAggregateUserRepository _userRepository;
        private IPfMembershipService _membershipService;

        public UserBuilder(
            IAggregateUserService userService, IReadOnlyAggregateUserRepository userRepository,
            IPfMembershipService membershipService)
        {
            _userService = userService;
            _userRepository = userRepository;
            _membershipService = membershipService;
        }

        public void Run()
        {
            LoggerSingleton.Get().Info("Create the default Root User");
            
            var users = _userRepository.Retreive(new List<UserRole>() { UserRole.Root });

            if (users.ToList().Count() < 1)
            {
                var membershipUser = _membershipService.GetUserByEmail("aleksjones@gmail.com");
                if (membershipUser != null)
                {
                    _membershipService.DeleteUser(membershipUser.UserName);
                }

                var identityuser1 = new IdentityProfileChange
                {
                    AccountStatus = AccountStatus.Active,
                    AccountLevel = AccountLevel.NotApplicable,
                    UserRole = UserRole.Root,
                    FirstName = "Aleksandr",
                    LastName = "Jones",
                };

                var membershipuser1 = new PfCreateNewMembershipUserRequest
                {
                    Email = "aleksjones@gmail.com",
                    Password = "123456",
                    IsApproved = true,
                    PasswordQuestion = "First Karate Teacher",
                    PasswordAnswer = "Sugiyama",
                };

                string outstatus1;
                var user = _userService.Create(membershipuser1, identityuser1, out outstatus1);

                if (user == null)
                {
                    throw new Exception("Failed to create Root User: " + outstatus1);
                }
            }
        }
    }
}