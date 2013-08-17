namespace Pleiades.Web.Security.Interface
{
    public interface IPasswordServices
    {
        bool IsValid(string password);
        string EncodePassword(string password);
    }
}
