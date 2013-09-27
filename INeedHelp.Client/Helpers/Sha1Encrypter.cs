using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace INeedHelp.Client.Helpers
{
    public class Sha1Encrypter
    {
        public static string ConvertToSha1(string password)
        {
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);
            HashAlgorithmProvider hashAlgorithm = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
            IBuffer hashBuffer = hashAlgorithm.HashData(buffer);

            var strHashBase64 = CryptographicBuffer.EncodeToBase64String(hashBuffer);
            return strHashBase64;
        }
    }
}
