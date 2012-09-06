namespace Pleiades.Web.Security.Model
{
    public enum UserRole
    {
        Anonymous = 0,
        Trusted = 1,
        Admin = 2,
        Supreme = 3,   
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
