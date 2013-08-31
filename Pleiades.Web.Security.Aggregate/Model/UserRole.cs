namespace Pleiades.Web.Security.Model
{
    public enum UserRole
    {
        Anonymous = 0,
        Trusted = 1,
        Admin = 2,
        Root = 3,   
    };

    public static class UserRoleExtensions
    {
        public static bool IsSupreme(this UserRole role)
        {
            return (role == UserRole.Root);
        }

        public static bool IsNotSupreme(this UserRole role)
        {
            return (role != UserRole.Root);
        }

        public static bool IsAdministratorOrSupreme(this UserRole role)
        {
            return (role == UserRole.Admin || role == UserRole.Root);
        }

        public static bool IsNotAdministratorOrSupreme(this UserRole role)
        {
            return !(role.IsAdministratorOrSupreme());
        }
    }
}
