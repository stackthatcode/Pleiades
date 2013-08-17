namespace Pleiades.Web.Security.Interface
{
    /// <summary>
    /// Enables us to abstract and mock the Forms Authentication functions
    /// </summary>
    public interface IFormsAuthenticationService
    {
        void SetAuthCookieForUser(string username, bool persistent);
        void ClearAuthenticationCookie();
    }
}