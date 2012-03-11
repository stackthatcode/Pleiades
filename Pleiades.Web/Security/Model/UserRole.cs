using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Utilities.General;

namespace Pleiades.Web.Security.Model
{
    public enum UserRole
    {
        Anonymous = 1,
        Trusted = 2,
        Admin = 3,
        Root = 4,   
    };

    public static class UserRoleExtensions
    {
        public static bool IsAdministrator(this UserRole role)
        {
            return (role == UserRole.Admin || role == UserRole.Root);
        }

        public static bool IsNotAdministrator(this UserRole role)
        {
            return !(role.IsAdministrator());
        }
    }
}
