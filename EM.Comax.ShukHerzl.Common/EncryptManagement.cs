using System.Security.Cryptography;
using System.Text;

namespace EM.Comax.ShukHerzl.Common
{
    public class EncryptManagement
    {
        private const string encryptionKey = "kYpn_6suv9y$B&E)H@1l";
        private static readonly byte[] Pbkdf2Salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
        private const int Pbkdf2Iterations = 1000;

        public static string PasswordEncrypt(string text)
        {
            byte[] bytesBuff = Encoding.Unicode.GetBytes(text);
            using (Aes aes = Aes.Create())
            {
                using Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(encryptionKey, Pbkdf2Salt, Pbkdf2Iterations, HashAlgorithmName.SHA1);
                aes.Key = crypto.GetBytes(32);
                aes.IV = crypto.GetBytes(16);
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cStream.Write(bytesBuff, 0, bytesBuff.Length);
                        cStream.Close();
                    }
                    text = Convert.ToBase64String(mStream.ToArray());
                }
            }
            return text;
        }

        public static string PasswordDecrypt(string cryptTxt)
        {
            cryptTxt = cryptTxt.Replace(" ", "+");
            int mod4 = cryptTxt.Length % 4;
            if (mod4 > 0)
            {
                cryptTxt += new string('=', 4 - mod4);
            }
            byte[] bytesBuff = Convert.FromBase64String(cryptTxt);
            using (Aes aes = Aes.Create())
            {
                using Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(encryptionKey, Pbkdf2Salt, Pbkdf2Iterations, HashAlgorithmName.SHA1);
                aes.Key = crypto.GetBytes(32);
                aes.IV = crypto.GetBytes(16);
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cStream.Write(bytesBuff, 0, bytesBuff.Length);
                        cStream.Close();
                    }
                    cryptTxt = Encoding.Unicode.GetString(mStream.ToArray());
                }
            }
            return cryptTxt;
        }
    }
}
