namespace Pleiades.Web.Security.Interface
{
    public interface IEncryptionService
    {
        string MakeHMAC256Base64(string input);
        string AESEncryptToBase64(string input);
        string AESDecryptBase64(string input);
    }
}
