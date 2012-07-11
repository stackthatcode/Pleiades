﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pleiades.Framework.Helpers;

namespace Pleiades.Framework.Identity.Model
{
    public enum UserRole
    {
        Anonymous = 1,
        Trusted = 2,
        Admin = 3,
        Supreme = 4,   
    };

    public static class UserRoleExtensions
    {
        public static bool IsAdministrator(this UserRole role)
        {
            return (role == UserRole.Admin || role == UserRole.Supreme);
        }

        public static bool IsNotAdministrator(this UserRole role)
        {
            return !(role.IsAdministrator());
        }
    }
}