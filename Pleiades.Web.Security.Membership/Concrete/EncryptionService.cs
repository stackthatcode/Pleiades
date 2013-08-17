using System;
using Pleiades.Web.Security.Interface;

namespace Pleiades.Web.Security.Concrete
{
    public class EncryptionService : IEncryptionService
    {
        private readonly string HMACKey;
        private readonly string AESKey;
        private readonly string AESInitializationVector;


        public string MakeHMAC256Base64(string input)
        {
            throw new NotImplementedException();
        }

        public string AESEncryptToBase64(string input)
        {
            throw new NotImplementedException();
        }

        public string AESDecryptBase64(string input)
        {
            throw new NotImplementedException();
        }
    }
}
