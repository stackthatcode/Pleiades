using System;
using System.Collections.Generic;
using Pleiades.Execution;
using Pleiades.Web.Security.Model;

namespace Pleiades.Web.Security.Execution.Context
{
    public class AuthenticateUserByRoleContext : IStepContext
    {
        public bool IsExecutionStateValid { get; set; }
        public string Message { get; set; }

        public string AttemptedUserName { get; set; }
        public string AttemptedPassword { get; set; }

        public List<UserRole> ExpectedRoles { get; set; }
        public bool PersistenceCookie { get; set; }

        public AuthenticateUserByRoleContext()
        {
            this.IsExecutionStateValid = true;
        }
    }
}
