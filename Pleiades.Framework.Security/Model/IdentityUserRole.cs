using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Framework.Helpers;

namespace Pleiades.Framework.Identity.Model
{
    public enum IdentityUserRole
    {
        Anonymous = 1,
        Trusted = 2,
        Admin = 3,
        Supreme = 4,   
    };

    public static class UserRoleExtensions
    {
        public static bool IsAdministrator(this IdentityUserRole role)
        {
            return (role == IdentityUserRole.Admin || role == IdentityUserRole.Supreme);
        }

        public static bool IsNotAdministrator(this IdentityUserRole role)
        {
            return !(role.IsAdministrator());
        }
    }
}
